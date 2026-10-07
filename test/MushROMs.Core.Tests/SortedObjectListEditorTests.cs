namespace MushROMs.Core.Tests;

using Maseya.Smas.Smb1.AreaData.ObjectData;

using Xunit;

public class SortedObjectListEditorTests
{
    [Fact]
    public void TestObjectMove()
    {
        var objectData = new AreaObjectCommand[]
        {
            new AreaObjectCommand(0x00, 0x00),
            new AreaObjectCommand(0x00, 0x01),
            new AreaObjectCommand(0x00, 0x02),
            new AreaObjectCommand(0x01, 0x03),

            new AreaObjectCommand(0x10, 0x04),
            new AreaObjectCommand(0x11, 0x05),
            new AreaObjectCommand(0x10, 0x06),

            new AreaObjectCommand(0x20, 0x07),

            new AreaObjectCommand(0x45, 0x08),

            new AreaObjectCommand(0x70, 0x89),

            new AreaObjectCommand(0x58, 0x0A),
        };

        var sortedObjectData = new SortedObjectListEditor(objectData);
        Assert.True(false, "I switched to indexed data. So update the tests to use indices");
        /*
        Assert.Equal(
            expected: objectData.Length,
            actual: sortedObjectData.Count);

        var nullableSelectedObject = sortedObjectData.AtCoords(0, 0);

        var selectedObject = nullableSelectedObject!.Value;
        Assert.Equal(
            expected: objectData[2],
            actual: selectedObject.Command);

        var newObject = selectedObject;
        newObject.X = 1;
        sortedObjectData.Edit(selectedObject, newObject);

        nullableSelectedObject = sortedObjectData.AtCoords(1, 0);

        selectedObject = nullableSelectedObject!.Value;
        Assert.Equal(
            expected: newObject,
            actual: selectedObject);

        nullableSelectedObject = sortedObjectData.AtCoords(0, 0);

        selectedObject = nullableSelectedObject!.Value;
        Assert.Equal(
            expected: objectData[1],
            actual: selectedObject.Command);

        nullableSelectedObject = sortedObjectData.AtCoords(1, 0);

        selectedObject = nullableSelectedObject!.Value;
        Assert.Equal(
            expected: newObject,
            actual: selectedObject);

        newObject = selectedObject;
        newObject.X = 2;

        sortedObjectData.Edit(selectedObject, newObject);

        nullableSelectedObject = sortedObjectData.AtCoords(2, 0);

        selectedObject = nullableSelectedObject!.Value;
        Assert.Equal(
            expected: newObject,
            actual: selectedObject);
        */
    }
}
