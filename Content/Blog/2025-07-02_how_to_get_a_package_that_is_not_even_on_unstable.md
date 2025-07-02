---
title: Install a Nix Package from an Unmerged Pull Request  
published: 2025-07-02  
tags: [nix, tabby]  
lead: "When you can't wait for a PR to get merged, you can still get the package with a few lines of Nix."  
isDraft: false  
---

One of the big advantages of NixOS is the vast number of available packages—over 120,000 in the `nixpkgs` repository.  
The downside? It can take some time for a new version to appear after its official release. Depending on the package, its complexity, and the maintainer, it might take several days (weeks or months) before it's available through the (at least) unstable channel.

## The issue

But sometimes, you need it fast. In this article, I'll show how to get a package that already has a pull request in the `nixpkgs` repository, but hasn’t been merged yet.

Obviously, there are risks involved—and often good reasons why the PR hasn't been merged—but in some cases, the package may work just fine on your platform while other platforms are still being fixed.

### Not about avoiding the Nix way

This post isn't about avoiding Nix packages or hacking around how things are supposed to work. It just shows how to fetch a package from an unmerged pull request into your local Nix store.

## Tabby terminal example

Tabby Terminal is a feature-rich terminal emulator with a nice UI for managing settings.  
Unfortunately, at the time of writing, it's not yet in the official `nixpkgs` repository—but there's already a PR open:  
https://github.com/NixOS/nixpkgs/pull/368048

![alt text](media/2025-07-02_how_to_get_a_package_that_is_not_even_on_unstable/img.png)

My platform (`x86_64-linux`) appears to be supported, so I gave it a try.

## Nix packages overlay

There are several ways to pull in a package from a PR. I chose the overlay method because it's simple to set up using flakes.

First, add an input:

```nix
# flake.nix
inputs = {
  nixpkgs-tabby.url = "github:geodic/nixpkgs/tabby"; # repo where the tabby package is already added
};
```

The URL points to user `geodic`'s fork, where the Tabby package has already been added. We're using it as an input.

Next, create an overlay module for Tabby. Add it to the `let-in` section of the `outputs` attribute set:

```nix
# flake.nix
outputs = {
  self,
  nixpkgs,
  ...
} @ inputs: let
  inherit (self) outputs;

  overlay-module = {
    nixpkgs.overlays = [
      (final: prev: {
        tabby = import inputs.nixpkgs-tabby {
          config.allowUnfree = true;
          localSystem = { inherit (prev) system; };
        };
      })
    ];
  };
in {
  # ...
};
```

## Import overlay as module

Then, import the module—preferably in your `home-manager` configuration. For example:

```nix
# flake.nix - inside the outputs attribute set
in {
  homeConfigurations = {
    username = home-manager.lib.homeManagerConfiguration {
      pkgs = nixpkgs.legacyPackages."${system}";
      modules = [ overlay-module ]; # overlay module
    };
  };
}
```
## Usage

And how do you use it?

```nix
# home.nix
home.packages = [ pkgs.tabby.tabby-terminal ];
```

Bang! Done.

## Conclusion

And that’s how you can install a Nix package from an unmerged pull request.

The Tabby PR will likely be merged eventually, but the core approach stays the same—useful for any package that’s still waiting in the queue.


Thanks to [geodic](https://github.com/geodic) for doing the work of adding Tabby to a fork of `nixpkgs` and making it available the Nix way—even before it’s officially merged.
