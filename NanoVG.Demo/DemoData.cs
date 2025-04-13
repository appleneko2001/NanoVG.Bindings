namespace NanoVG.Demo;

public struct DemoData
{
    internal int fontNormal, fontBold, fontIcons, fontEmoji;
    internal int[] images = new int [12];

    public DemoData()
    {
        fontNormal = 0;
        fontBold = 0;
        fontIcons = 0;
        fontEmoji = 0;
    }
}