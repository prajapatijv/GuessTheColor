using System.Windows.Media;
using Framework.MVVM;
using System.Diagnostics;

namespace GuessTheColor
{
    public class Field : ViewModelBase
    {
        public static Field NewField(byte id)
        {
            var field = new Field();
            field.Id = id;
            field.State = FieldStateEnum.Empty;

            Color color = System.Windows.Media.Color.FromArgb(10, 255, 255, 255);

            field.HintColor = new SolidColorBrush(color);
            field.hintType = HintEnum.NotSet;
            return field;
        }

        public static Field NewField(byte id, Brush color)
        {
            var field = NewField(id);

            field.HeaderColor = new SolidColorBrush(Colors.Transparent);
            field.headerText = "?";
            field.Color = color;
            field.hintType = HintEnum.NotSet;

            return field;
        }

        public byte Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public FieldStateEnum State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                this.OnPropertyChanged("State");
            }
        }

        public Brush Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                InnerColor = (color as SolidColorBrush).Color;
                this.OnPropertyChanged("Color");
            }
        }

        public Color InnerColor { get; private set; }

        public Brush HintColor
        {
            get
            {
                return hintColor;
            }
            set
            {
                hintColor = value;
                this.OnPropertyChanged("HintColor");
            }
        }

        public HintEnum HintType
        {
            get
            {
                return hintType;
            }

            set
            {
                // Set Hint Color
                hintType = value;
                Brush hintcolor = new SolidColorBrush(Colors.Transparent);
                switch (hintType)
                {
                    case HintEnum.NotSet:
                        hintcolor = new SolidColorBrush(Colors.Transparent);
                        break;
                    case HintEnum.WrongColor:
                        hintcolor = new SolidColorBrush(Colors.Red);
                        break;
                    case HintEnum.RightColorWrongPosition:
                        hintcolor = new SolidColorBrush(Colors.Yellow);
                        break;
                    case HintEnum.RightColorRightPosition:
                        hintcolor = new SolidColorBrush(Colors.Green);
                        break;
                    default:
                        break;
                }

                this.hintColor = hintcolor;
                this.OnPropertyChanged("HintColor");

            }
        }

        public Brush HeaderColor
        {
            get
            {
                return headerColor;
            }
            set
            {
                headerColor = value;
                this.OnPropertyChanged("HeaderColor");
            }
        }

        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
                this.OnPropertyChanged("HeaderText");
            }
        }

        private byte id;
        private FieldStateEnum state;
        private Brush color;
        private Brush hintColor;
        private Brush headerColor;
        private string headerText;
        private HintEnum hintType;
    }
}
