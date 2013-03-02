using System;

namespace LilyPathDemo
{
    public class TestSheetAttribute : Attribute
    {
        public string Name { get; set; }

        public TestSheetAttribute (string name)
        {
            Name = name;
        }
    }
}
