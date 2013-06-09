using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;

namespace GuessTheColor
{
    public class GameColors
    {
        Dictionary<int, Brush> colors;
        public GameColors()
        {
            colors = new Dictionary<int, Brush>();

            colors.Add(1, new SolidColorBrush(Colors.Blue));
            colors.Add(2, new SolidColorBrush(Colors.Orange));
            colors.Add(3, new SolidColorBrush(Colors.Yellow));
            colors.Add(4, new SolidColorBrush(Colors.Red));
            colors.Add(5, new SolidColorBrush(Colors.Green));
            colors.Add(6, new SolidColorBrush(Colors.Purple));
        }

        public void NewGameRandomColors(Row headerRow)
        {
            List<int> selectedNumbers = new List<int>();
            Random r = new Random(DateTime.Now.Millisecond);

            while (true)
            {
                var number = r.Next(1, 6);

                if (!selectedNumbers.Contains(number) && number != 0)
                {
                    selectedNumbers.Add(number);
                }

                if (selectedNumbers.Count(x => x != 0) == 4)
                {
                    break;
                }
            }

            headerRow.Field1 = Field.NewField(1, colors[selectedNumbers[0]]);
            headerRow.Field2 = Field.NewField(2, colors[selectedNumbers[1]]);
            headerRow.Field3 = Field.NewField(3, colors[selectedNumbers[2]]);
            headerRow.Field4 = Field.NewField(4, colors[selectedNumbers[3]]);
        }

        public Brush Color1 { get { return colors[1]; } }
        public Brush Color2 { get { return colors[2]; } }
        public Brush Color3 { get { return colors[3]; } }
        public Brush Color4 { get { return colors[4]; } }
        public Brush Color5 { get { return colors[5]; } }
        public Brush Color6 { get { return colors[6]; } }
    }

    public enum HintEnum
    {
        NotSet = 0,
        WrongColor = 1,
        RightColorWrongPosition = 2,
        RightColorRightPosition = 3
    }

    public enum FieldStateEnum
    {
        Empty = 0,
        Filled = 1
    }

    public enum StateEnum
    {
        NewGame = 0,
        FieldEmpty = 1,
        FieldFilled = 2,
        RowFilled = 3,
        DuplicateColor = 4,
        GameOverFailure = 5,
        GameOverSuccess = 6,
        NotSet = 7
    }
}
