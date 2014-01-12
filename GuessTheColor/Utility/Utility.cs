using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.IO;
using Framework.MVVM;

namespace GuessTheColor
{
    public class GameColors
    {
        Dictionary<int, Brush> colors;
        public GameColors()
        {
            colors = new Dictionary<int, Brush>();

            colors.Add(1, new SolidColorBrush(Colors.Blue));
            colors.Add(2, new SolidColorBrush(Colors.Cyan));
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

    public class Time
    {
        public Time(byte hour, byte minute, byte second)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        public byte Hour { get; set; }
        public byte Minute { get; set; }
        public byte Second { get; set; }

        public Time Increment()
        {
            this.Second += 1;
            if (this.Second == 60)
            {
                this.Second = 0;
                this.Minute += 1;
            }
            if (this.Minute == 60)
            {
                this.Minute = 0;
                this.Hour += 1;
            }

            return new Time(this.Hour, this.Minute, this.Second);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", Hour.ToString("D2"), Minute.ToString("D2"), Second.ToString("D2"));
        }   
    }

    public class Score : ViewModelBase
    {
        public string PlayerName
        {
            get
            {
                return _PlayerName;
            }
            set
            {
                _PlayerName = value;
                this.OnPropertyChanged("PlayerName");
            }
        }
        private string _PlayerName { get; set; }
        public Time ScoreTime 
                {
            get
            {
                return _ScoreTime;
            }
            set
            {
                _ScoreTime = value;
                this.OnPropertyChanged("ScoreTime");
            }
        }
        private Time _ScoreTime { get; set; }

        public Score()
        {
            this._PlayerName = "FunUnlocker!";
            this.Reset();
        }

        public void Reset()
        {
            this.ScoreTime = new Time(0, 0, 0) ;
        }

        public void Increment()
        {
            this.ScoreTime = this.ScoreTime.Increment();
        }
    }

    public class Scores
    {
        private List<Score> scores;
       
        public Score HighestScore { get; private set; }

        public Score CurrentScore { get; private set; }

        public Scores()
        {
            this.scores = new List<Score>();
            this.CurrentScore = new Score();
        }

        public void Add(Score score)
        {
            this.scores.Add(score);
        }

        public void Save(Score score)
        {
            var store = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("score.txt", FileMode.OpenOrCreate, store));
            Writer.WriteLine("tst");
            Writer.Close();
        }

        private void Load()
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("score.txt", FileMode.Open, fileStorage));
                string textFile = Reader.ReadToEnd();
                var x = textFile;
                Reader.Close();
            }
            catch
            {
                //Do nothing
            }
        }
    }
}
