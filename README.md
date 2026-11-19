# CSE 310 — Applied Programming (C#)

My C# work for CSE 310 (Applied Programming) at BYU–Pathway Worldwide. Each folder is one module with its own README and demo video.

| Project | What it is |
| --- | --- |
| [`HelloWorld/`](HelloWorld/) | Sets up the .NET development environment with a first console program ([demo](https://www.loom.com/share/9b83a50f32104e0287fc964ed455526b)) |
| [`InventorySystem/`](InventorySystem/) | Console inventory manager for stock levels and expiry. It tracks quantity, price, shelf location and reorder level, and saves to `inventory.txt` between runs. Shows classes, abstract classes, inheritance (`InventoryItem` → `Product` and `PerishableProduct`) and a struct (`StockLocation`) ([demo](https://youtu.be/9yk7HAJ8_zM)) |

## Run a project

Requires the [.NET SDK](https://dotnet.microsoft.com/download).

```bash
cd InventorySystem
dotnet run
```

`AppliedProgramming.slnx` opens both projects together in Visual Studio or VS Code (C# Dev Kit).

## Related

The Python/pandas module of this course is in [Data-Analysis-with-Python-and-Pandas](https://github.com/hlaolil/Data-Analysis-with-Python-and-Pandas).
