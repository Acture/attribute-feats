"""Validate final text, all feat icon mappings, blueprint identity, and release contents."""
import argparse
import collections
import hashlib
import json
import re
import struct
import subprocess
import zipfile
from pathlib import Path
from feat_catalog import ROOT, catalog, calls, split_args, STRING


def read(path):
    return path.read_text(encoding="utf-8-sig")


def normalize(expr):
    return re.sub(r'\s+', '', expr)


def mechanics(source):
    # Compare actual configurator mutations, excluding UI text and artwork.
    results = []
    for match in re.finditer(r'\.(Add\w+|Set\w+)\(', source):
        method = match.group(1)
        if method in ('SetDisplayName', 'SetDescription', 'SetIconIfPresent'):
            continue
        body = next(calls(source[match.start():], method))[0]
        results.append((method, normalize(body)))
    for body, _, _ in calls(source, 'New'):
        args = split_args(body)
        if len(args) >= 2:
            results.append(('New', normalize(','.join(args[:2]))))
    return results


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--allow-pending-icons', action='store_true')
    parser.add_argument('--release', type=Path)
    args = parser.parse_args()
    rows = catalog()
    resources = json.loads(read(ROOT / 'Localization/FeatText.json'))
    entries = {r['Key']: r for r in resources}
    assert len(entries) == len(resources) == 196
    for entry in resources:
        assert entry['enGB'].strip() and entry['zhCN'].strip(), entry['Key']
        assert re.findall(r'\{\w+\}', entry['enGB']) == re.findall(r'\{\w+\}', entry['zhCN']), entry['Key']
        assert re.findall(r'</?\w+>', entry['enGB']) == re.findall(r'</?\w+>', entry['zhCN']), entry['Key']
    for row in rows:
        assert entries[row['key'] + '.Name']['enGB'] == row['en'], row['internal']
        assert entries[row['key'] + '.Name']['zhCN'] == row['zh'], row['internal']
        assert row['key'] + '.Lore' in entries
    for lang in ['en', 'zh']:
        names = [r[lang].casefold() for r in rows]
        assert len(set(names)) == 92, f'duplicate {lang} feat name'

    for path in (ROOT / 'New_Feats').glob('*.cs'):
        if path.name in ('Common.cs', 'IconLoader.cs', 'FeatTextCatalog.cs', 'FeatRegistry.cs'):
            continue
        baseline = subprocess.run(['git','show',f'6887a8a:New_Feats/{path.name}'], cwd=ROOT,
                                  check=True, capture_output=True, encoding='utf-8').stdout
        assert mechanics(read(path)) == mechanics(baseline), f'mechanics/blueprint identity changed: {path.name}'
        # Includes all inline GUIDs and component TypeId identities.
        pattern = r'"[0-9a-fA-F-]{32,36}"'
        assert re.findall(pattern, read(path)) == re.findall(pattern, baseline), path.name
    baseline_guids = subprocess.run(['git','show','d4de92a:New_Feats/Guids.cs'],cwd=ROOT,check=True,capture_output=True,encoding='utf-8').stdout
    assert normalize(read(ROOT/'New_Feats/Guids.cs')) == normalize(baseline_guids)

    manifest = json.loads(read(ROOT / 'docs/icon-manifest.json'))
    assert len(manifest) == len({r['internal'] for r in manifest}) == len({r['filename'] for r in manifest}) == 92
    assert {r['internal'] for r in manifest} == {r['internal'] for r in rows}
    aliases = dict(re.findall(r'\{ "([^"]+)", "([^"]+)" \}', read(ROOT/'New_Feats/IconLoader.cs')))
    pending = []
    hashes = []
    for asset in manifest:
        path = ROOT/'Icons'/asset['filename']
        if not path.exists():
            pending.append(asset['filename']); continue
        data = path.read_bytes()
        assert data[:8] == b'\x89PNG\r\n\x1a\n' and struct.unpack('>II',data[16:24]) == (128,128), path
        digest = hashlib.sha256(data).hexdigest()
        assert asset['sha256'] == digest, path
        hashes.append(digest)
        if asset['origin'] == 'built-in image_gen':
            assert asset['prompt'].strip() and asset['generatedSource'].endswith('.png'), path
        assert aliases.get(asset['internal'],asset['internal']) == path.stem, asset['internal']
    if not args.allow_pending_icons:
        assert not pending, f'missing icons: {pending}'
        assert len(set(hashes)) == 92, 'duplicate icon images'
        assert {p.name for p in (ROOT/'Icons').glob('*.png')} == {r['filename'] for r in manifest}, 'extra icon files'
    if args.release:
        assert not pending
        with zipfile.ZipFile(args.release) as archive:
            names = archive.namelist()
            icons = {n.replace('\\','/') for n in names if n.endswith('.png')}
            assert icons == {'Icons/'+r['filename'] for r in manifest}, 'release icon coverage'
            assert {Path(n).name for n in names if n.endswith('.dll')} == {'AttributeFeats.dll'}, 'game DLLs must not ship'
            assert json.loads(archive.read('Info.json'))['Version'] == '0.1.2'
            for asset in manifest:
                assert archive.read('Icons/'+asset['filename']) == (ROOT/'Icons'/asset['filename']).read_bytes()
    print(f'92 feats, 196 bilingual entries, unique names, stable blueprint identities and unchanged mechanics verified; {92-len(pending)}/92 icons present.')
    if args.release: print(f'Release ZIP verified: {args.release}')


if __name__ == '__main__':
    main()
