# __Screw management documentation__
## __Taking Screws__
To take a screw from the tile the robot is standing on, you can use the `TakeScrew` method.

```
TakeScrew()
```

## __Placing Screws__
To Place a screw on the tile the robot is standing on, you can use the `PlaceScrew` method:

```
PlaceScrew()
```

## __Checks__
You may check whether a screw is on the current tile with the `ScrewThere` method:

```
ScrewThere()
```

This method returns True or False, and should be used in other statements like so:

```
if ScrewThere():
    GoForward()
```