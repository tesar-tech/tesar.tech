---
title: Rename photos tool
published: 2020-05-25
tags: [dotnet, powershell, programming]
---

I made a simple dotnet global tool that helps renaming my photos by EXIF date taken metadata.

Original: `DSC106.jpg` => New: `20200312_2345.jpg`

It is written in C#, but the core functionality is powershell script.

https://github.com/tesar-tech/RenamePhotos

## Install
> dotnet tool install renamephotos -g

[![RenamePhotos](https://img.shields.io/nuget/v/RenamePhotos.svg)](https://www.nuget.org/packages/RenamePhotos/)

## Usage

Use `RenamePhotos` command in folder with `.jpg`  pictures.

---
