"""Export the public name/icon catalog without internal authoring records."""
from feat_catalog import ROOT, catalog


def main():
    rows = catalog()
    lines = [
        '# Feat names and icons', '',
        'The 92 existing feats below have individual icons. The six Weapon Damage '
        'feats also have English/Chinese names and descriptions, but no bespoke icons yet.', '',
        '| Family | Stable internal name | English | 简体中文 |',
        '|---|---|---|---|',
    ]
    for row in rows:
        lines.append(f"| {row['family']} | `{row['internal']}` | {row['en']} | {row['zh']} |")
    lines += ['', 'See [the contact sheet](feat-icons.png) and [validation](validation.md).', '']
    (ROOT / 'docs/feat-catalog.md').write_text('\n'.join(lines), encoding='utf-8')
    print('Updated docs/feat-catalog.md.')


if __name__ == '__main__':
    main()
