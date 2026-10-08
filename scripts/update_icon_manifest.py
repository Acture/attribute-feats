"""Refresh public icon names/hashes from production assets; never copy provenance."""
import hashlib
import json
import re
from feat_catalog import ROOT, PROJECT, catalog


def main():
    source = (PROJECT.parent / 'ACHomebrew.Core/IconLoader.cs').read_text(encoding='utf-8-sig')
    aliases = dict(re.findall(r'\{ "([^"]+)", "([^"]+)" \}', source))
    manifest = []
    for row in catalog():
        filename = aliases.get(row['internal'], row['internal']) + '.png'
        data = (PROJECT / 'Icons' / filename).read_bytes()
        manifest.append(dict(internal=row['internal'], family=row['family'],
                             filename=filename, nameEn=row['en'], nameZh=row['zh'],
                             sha256=hashlib.sha256(data).hexdigest()))
    (ROOT / 'doc/icon-manifest.json').write_text(
        json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print('Updated 92 public icon mappings and hashes.')


if __name__ == '__main__':
    main()
