# __Chip management documentation__
## __Taking Chips__
To take a chip from the tile the robot is standing on, you can use the `TakeChip` method.

```
TakeChip()
```

## __Placing Chips__
To Place a chip on the tile the robot is standing on, you can use the `PlaceChip` method:

```
PlaceChip()
```

## __Checks__
You may check whether a chip is on the current tile with the `ChipThere` method:

```
ChipThere()
```

This method returns True or False, and should be used in other statements like so:

```
if ChipThere():
    GoForward()
```