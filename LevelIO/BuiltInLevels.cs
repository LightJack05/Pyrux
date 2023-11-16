namespace Pyrux.LevelIO;
public static class BuiltInLevels
{
    public static void ConstructLevels()
    {
        List<PyruxLevel> pyruxLevels = new List<PyruxLevel>()
        {
            new(
                "Demo 01",
                "DemoTask",
                true,
                new(
                    new bool[2, 2]
                    {
                        { false, false},
                        {false, false }
                    },
                    new int[2, 2]
                    {
                        {0,0},
                        {0,0 }
                    },
                    new(0,0),
                    0
                ),
            "")
        };

        LevelIO.LevelSaving.Save(pyruxLevels);
    }
}