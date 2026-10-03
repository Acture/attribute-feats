# AttributeFeats development

## Documentation

- Read [README.md](README.md) for the project and the complete notes workflow.
- Internal design, balance proposals, mod comparisons and testing research belong
  in [notes/attribute-feats/首页.md](notes/attribute-feats/首页.md) and its linked
  documents. This is a private submodule, not a directory of files to commit in
  the public code repository. Linear remains the source for tasks and status.
- The submodule path is `notes`, its remote is
  `https://github.com/Acture/obsidian-vault.git`, and the editable project branch is
  `project/attribute-feats`. Only change `attribute-feats/` in that repository.
- Initialize the pinned version with `git submodule update --init --recursive -- notes`.
  Inspect the actual commit, branch and worktree before editing; initialization
  may leave detached HEAD. Preserve unpublished commits and uncommitted changes.
- Follow the central vault's
  [project onboarding guide](https://github.com/Acture/obsidian-vault/blob/master/项目接入.md).
  Install its mandatory push boundary hook in each notes clone as described in
  README, use UTF-8 Python on Windows, and never bypass a failing hook. Do not
  merge the entire vault master into the project branch or rewrite its history.
- Commit and successfully push notes to their project branch before staging the
  parent repository's `notes` gitlink. Verify the notes HEAD is reachable from the
  published branch. Never commit a pointer to an unpublished local notes commit.
- Public usage, installation, build instructions and release notes stay in the
  public README and CHANGELOG. Code builds and public CI must work without the
  private submodule. Do not enable recursive checkout for public CI merely to
  obtain internal design notes.
- Existing documents and any legacy doc branch remain until content and history
  are accounted for. Preserve unrelated local modifications during migrations.

## Validation

- Keep historical research observations distinct from current implementation and
  test evidence. Static repository checks do not prove feats work in the game.
- Use CLI and code-based checks; do not use computer use or launch a game as part
  of a documentation-only change.
