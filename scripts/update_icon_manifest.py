"""Refresh public icon names/hashes from production assets; never copy provenance."""
import hashlib
import json
import re
from feat_catalog import ROOT, PROJECT, catalog


def main():
    source = (PROJECT.parent / 'ACHomebrew.Core/IconLoader.cs').read_text(encoding='utf-8-sig')
    aliases = dict(re.findall(r'\{ "([^"]+)", "([^"]+)" \}', source))
    manifest = []
    rows = catalog()
    for row in rows:
        filename = aliases.get(row['internal'], row['internal']) + '.png'
        data = (PROJECT / 'Icons' / filename).read_bytes()
        manifest.append(dict(internal=row['internal'], family=row['family'],
                             filename=filename, nameEn=row['en'], nameZh=row['zh'],
                             sha256=hashlib.sha256(data).hexdigest()))
    # Newer feats are not in the published catalog; keep their curated entries and refresh hashes.
    path = ROOT / 'doc/icon-manifest.json'
    known = {row['internal'] for row in rows}
    newer = [entry for entry in json.loads(path.read_text(encoding='utf-8')) if entry['internal'] not in known]
    for entry in newer:
        entry['sha256'] = hashlib.sha256((PROJECT / 'Icons' / entry['filename']).read_bytes()).hexdigest()
    manifest += newer
    path.write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(f'Updated {len(rows)} catalog and {len(newer)} newer icon mappings and hashes.')


if __name__ == '__main__':
    main()
