# attribute feats

A small mod that adds scaling bonuses with respect to attributes.

Thanks to @CasDragon for his code snippets and idea. Expanded from Redditor class feat.

And I hate Visual Studio.

---

## Releasing (CI/CD)

Releases are fully automated via GitHub Actions — no local build needed, no separate repo to maintain.

The workflow uses **[DepotDownloader](https://github.com/SteamRE/DepotDownloader)** to pull the game's managed DLLs directly from Steam at build time, and downloads **[Unity Mod Manager](https://github.com/newman55/unity-mod-manager)** from its GitHub releases automatically. Whenever the game or UMM updates, the next release picks up the new files with zero manual work.

### One-time setup: Steam secrets

Add two **repository secrets** to this repo
(`Settings → Secrets and variables → Actions → New repository secret`):

| Secret name      | Value                        |
|------------------|------------------------------|
| `STEAM_USERNAME` | Your Steam account username  |
| `STEAM_PASSWORD` | Your Steam account password  |

> **Tip:** It is recommended to use a separate Steam account (a "bot" account) that owns the game, so your main account credentials are not stored in GitHub.

### Publish a release

**Option A — tag push (recommended):**
```bash
git tag v0.1.0
git push origin v0.1.0
```
The workflow triggers automatically, downloads the latest game DLLs from Steam, builds, and creates a GitHub Release with the mod zip attached.

**Option B — manual dispatch:**
Go to `Actions → Release → Run workflow`, enter the version (e.g. `0.1.0`), and click **Run workflow**.

The version number is stamped into `Info.json` and `Repository.json` automatically at build time.