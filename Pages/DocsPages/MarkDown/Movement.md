# __Movement documentation__
## __Basics__
Pyrux uses methods to coordinate the robot's movement.
To move it, you have to call the methods, like so:

```
GoForward()
```

You may also call these methods in loops, methods or other structures:

```
for i in range(5):
    GoForward()
```

## __Available Movement Methods__
### Forward movement
Move the robot forward one square:

```
GoForward()
```

May throw a `Pyrux.WallAheadException` if the robot runs into a wall or border of the play field.
### Turning
Turn the robot left:

```
TurnLeft()
```

Turn the robot right:

```
TurnRight()
```

## __Checks__
### Check whether a wall is infront of the player

```
WallAhead()
```

This method will return True or False, use it like this in if statements:

```
if WallAhead():
    TurnLeft()
```

```
while not WallAhead():
    GoForward()
```