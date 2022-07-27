# kasthack.AsyncSynchronized

## What

kasthack.AsyncSynchronized is a library for making types with asynchronous methods thread safe without falling back to synchronous IO or manually writing proxies.

[![Github All Releases](https://img.shields.io/github/downloads/kasthack-labs/kasthack.AsyncSynchronized/total.svg)](https://github.com/kasthack-labs/kasthack.AsyncSynchronized/releases/latest)
[![GitHub release](https://img.shields.io/github/release/kasthack-labs/kasthack.AsyncSynchronized.svg)](https://github.com/kasthack-labs/kasthack.AsyncSynchronized/releases/latest)
[![license](https://img.shields.io/github/license/kasthack-labs/kasthack.AsyncSynchronized.svg)](LICENSE)
[![.NET Status](https://github.com/kasthack-labs/kasthack.AsyncSynchronized/workflows/.NET/badge.svg)](https://github.com/kasthack-labs/kasthack.AsyncSynchronized/actions?query=workflow%3A.NET)
[![Patreon pledges](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Fshieldsio-patreon.vercel.app%2Fapi%3Fusername%3Dkasthack%26type%3Dpledges&style=flat)](https://patreon.com/kasthack)
[![Patreon patrons](https://img.shields.io/endpoint.svg?url=https%3A%2F%2Fshieldsio-patreon.vercel.app%2Fapi%3Fusername%3Dkasthack%26type%3Dpatrons&style=flat)](https://patreon.com/kasthack)

## Why does this exist?

Originally, I needed thread-safe versions of `Stream`, `TextReader`, and 'TextWriter' that support async IO. It turns out that [`TextReader.Synchronized`](https://source.dot.net/#System.Private.CoreLib/TextReader.cs,382) and [`TextWriter.Synchronized`](https://source.dot.net/#System.Private.CoreLib/TextWriter.cs,861) create wrappers which internally fall back to synchronous operations, and `Stream.Synchronized` doesn't modify async behavior at all. So, I wrote this library that wraps objects into dynamic proxies that enter a `SemaphoreSlim` on every call.

## Implementation

Current version is a PoC whiich relies on Castle.DynamicProxies, so

* It's not exactly fact as there's a ton of reflection under the hood on every call.
* Only virtual members or interface methods get synchronized.
* Non-virtual properties seem to be broken in the proxy library.
* Proxied types must have parameterless constructors(maybe, I've just missed a configuration option somewhere is Castle)

This library can be rewritten with compile-time code generators or at least with `Reflection.Emit`.

## Usage

1. Take an instance of a type with virtual/abstract members1
2. Call `.Synchronized()` extension method.
3. Enjoy thread safety.


### Properties

`.Synchronized(...)` doesn't create read locks on property getters by default. You can change this behavior by passing `false` in `allowGetters` parameter. 

### Examples

Check out `kasthack.AsyncSynchronized.Tests` project.