# attribute feats

A small mod that adds scaling bonuses with respect to attributes.

Thanks to @CasDragon for his code snippets and idea. Expanded from Redditor class feat.

And I hate Visual Studio.

---

## Releasing (CI/CD)

Releases are fully automated via GitHub Actions — no local build needed.

### One-time setup: game library secrets

The project references proprietary DLLs that ship with the game and cannot be committed here.
Store them in a **private** GitHub repository with this layout (mirroring the game install):

```
Wrath_Data/
  Managed/
    Assembly-CSharp.dll
    Assembly-CSharp-firstpass.dll
    Unity*.dll
    Core*.dll
    Owlcat*.dll
    Newtonsoft.Json.dll
    UniRx.dll               ← optional
    UnityModManager/
      UnityModManager.dll
      0Harmony.dll
```

Then add two **repository secrets** to this repo
(`Settings → Secrets and variables → Actions → New repository secret`):

| Secret name       | Value                                              |
|-------------------|----------------------------------------------------|
| `GAME_LIBS_REPO`  | `owner/repo` of the private game-libs repository   |
| `GAME_LIBS_TOKEN` | A GitHub PAT with **`repo` read** scope for it     |

### Publish a release

**Option A — tag push (recommended):**
```bash
git tag v0.1.0
git push origin v0.1.0
```
The workflow triggers automatically, builds, and creates a GitHub Release with the mod zip attached.

**Option B — manual dispatch:**
Go to `Actions → Release → Run workflow`, enter the version (e.g. `0.1.0`), and click **Run workflow**.

The version number is stamped into `Info.json` and `Repository.json` automatically at build time.