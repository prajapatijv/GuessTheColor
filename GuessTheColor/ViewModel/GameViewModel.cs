using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Framework.MVVM;
using System.Windows.Threading;
using System.Windows;

namespace GuessTheColor
{
    public class GameViewModel : ViewModelBase
    {
        private ObservableCollection<Row> rows;
        private ObservableCollection<Row> headerRow;
        private Row currentRow;
        private ICommand okCommand;
        private ICommand exitCommand;
        private ICommand resumeCommand;

        //private ICommand resetCommand;
        //private ICommand newGameCommand;

        private GameColors gameColors;
        private Scores _scores;

        //private Time timeTracker;
        private DispatcherTimer dtm = new DispatcherTimer();

        #region ctor

        public GameViewModel()
        {
            this._scores = new Scores();
            this._currentScore = new Score();

            this.gameColors = new GameColors();
            
            rows = new ObservableCollection<Row>();
            headerRow = new ObservableCollection<Row>();
            this.okCommand = new DelegateCommand(new Action<object>((p) => this.NewGame()));
            this.exitCommand = new DelegateCommand(new Action<object>((p) => Application.Current.Terminate()));
            this.resumeCommand = new DelegateCommand(new Action<object>((p) => this.resume()));

            //this.newGameCommand = new DelegateCommand(new Action<object>((p) => this.NewGame()));
            //this.resetCommand = new DelegateCommand(new Action<object>((p) => this.Reset()));

            this.NewGame();

            dtm.Interval = TimeSpan.FromSeconds(1);
            dtm.Tick += OnTimerTick;
        }

        void OnTimerTick(Object s, EventArgs e)
        {
            this.CurrentScore.Increment();
        }

        #endregion

        #region Property

        public Scores Scores {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
                this.OnPropertyChanged("Scores");
            }
        }

        public Score CurrentScore
        {
            get
            {
                return _currentScore;
            }
            set
            {
                _currentScore = value;
                this.OnPropertyChanged("CurrentScore");
            }
        }
        private Score _currentScore;

        public bool Paused
        {
            get
            {
                return this.paused;
            }
            set
            {
                this.paused = value;
                this.OnPropertyChanged("Paused");
            }
        }
        private bool paused;

        public StateEnum GameState
        {
            get
            {
                return this.gameState;
            }
            set
            {
                gameState = value;
                this.OnPropertyChanged("GameState");
                if (gameState == StateEnum.GameOverSuccess)
                {
                    this.dtm.Stop();
                    this.GameOverSuccess = true;
                    this.GameOverFailure = false;
                    this.GameOver = true;
                }
                else if (gameState == StateEnum.GameOverFailure)
                {
                    this.dtm.Stop();
                    this.GameOverSuccess = false;
                    this.GameOverFailure = true;
                    this.GameOver = true;
                }
                else
                {
                    this.GameOverSuccess = false;
                    this.GameOverFailure = false;
                    this.GameOver = false;
                }

            }
        }
        private StateEnum gameState;

        public bool GameOver
        {
            get
            {
                return this.gameOver;
            }
            set
            {
                this.gameOver = value;
                this.OnPropertyChanged("GameOver");
            }
        }
        private bool gameOver;

        public bool GameOverFailure
        {
            get
            {
                return !GameOverSuccess;
            }
            set
            {
                gameOverFailure = value;
                this.OnPropertyChanged("GameOverFailure");
            }
        }
        private bool gameOverFailure;

        public bool GameOverSuccess
        {
            get
            {
                var gameOver = this.gameState == StateEnum.GameOverSuccess;

                if (gameOver)
                {
                    var headerRow = this.HeaderRow.First();
                    headerRow.Field1.HeaderColor = headerRow.Field1.Color;
                    headerRow.Field2.HeaderColor = headerRow.Field2.Color;
                    headerRow.Field3.HeaderColor = headerRow.Field3.Color;
                    headerRow.Field4.HeaderColor = headerRow.Field4.Color;

                    headerRow.Field1.HeaderText = string.Empty;
                    headerRow.Field2.HeaderText = string.Empty;
                    headerRow.Field3.HeaderText = string.Empty;
                    headerRow.Field4.HeaderText = string.Empty;
                }

                return gameOver;

            }
            set
            {
                gameOverSuccess = value;
                this.OnPropertyChanged("GameOverSuccess");
            }
        }
        private bool gameOverSuccess;


        public ObservableCollection<Row> HeaderRow
        {
            get
            {
                return this.headerRow;
            }
        }

        public ObservableCollection<Row> Rows
        {
            get
            {
                return rows;
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return this.okCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return this.exitCommand;
            }
        }

        public ICommand ResumeCommand
        {
            get
            {
                return this.resumeCommand;
            }
        }

        
        //public ICommand NewGameCommand
        //{
        //    get
        //    {
        //        return this.newGameCommand;
        //    }
        //}

        //public ICommand ResetCommand
        //{
        //    get
        //    {
        //        return this.resetCommand;
        //    }
        //}

        public GameColors GameColors
        {
            get
            {
                return this.gameColors;
            }
        }

        #endregion

        #region Public Methods

        public void NewGame()
        {
            headerRow.Clear();

            // Add Header row with pre defined colors
            headerRow.Add(new Row().NewHeaderRow());

            this.Reset();
        }

        public void Reset()
        {
            rows.Clear();

            // Add Details Empty Rows
            for (byte i = 1; i <= 9; i++)
            {
                rows.Add(new Row().NewRow(i));
            }

            currentRow = rows.Last();
            this.GameState = StateEnum.NewGame;
            this.CurrentScore.Reset();
            this.dtm.Start();
        }

        public void Pause()
        {
            if (dtm.IsEnabled)
            {
                dtm.Stop();
                this.Paused = true;
            }
        }

        private void resume()
        {
            if (!dtm.IsEnabled)
            {
                dtm.Start();
                this.Paused = false;
            }
        }

        public void OnColorClick(Brush color)
        {
            if (this.Paused)
                return;

            StateEnum state = RowFilledState();

            switch (state)
            {
                case StateEnum.GameOverSuccess:
                    return;
                case StateEnum.RowFilled:
                    this.currentRow = this.rows.FirstOrDefault(x => x.Id == this.currentRow.Id - 1);
                    if (this.currentRow != null)
                    {
                        var clr = new SolidColorBrush(Colors.White);

                        OnColorClick(color);
                    }
                    else
                    {
                        return;
                    }

                    break;
            }


            // fill each field with color.
            if (state == StateEnum.FieldEmpty)
                state = SetFieldBrush(this.currentRow.Field1, color);
            if (state == StateEnum.FieldEmpty)
                state = SetFieldBrush(this.currentRow.Field2, color);
            if (state == StateEnum.FieldEmpty)
                state = SetFieldBrush(this.currentRow.Field3, color);
            if (state == StateEnum.FieldEmpty)
            {
                state = SetFieldBrush(this.currentRow.Field4, color);
                // Get Hint
                var headerRow = this.headerRow.First();
                GetHint(headerRow.Field1, this.currentRow.Field1);
                GetHint(headerRow.Field2, this.currentRow.Field2);
                GetHint(headerRow.Field3, this.currentRow.Field3);
                GetHint(headerRow.Field4, this.currentRow.Field4);

                // Check Game over condition
                if (IsGameOver())
                {
                    state = StateEnum.GameOverSuccess;
                    this.GameState = StateEnum.GameOverSuccess;
                    // Game over
                }
                else if (this.currentRow.Id == this.rows.Min(i=> i.Id))
                {
                    state = StateEnum.GameOverFailure;
                    this.GameState = StateEnum.GameOverFailure;
                    // Game over
                }
            }
        }

        #endregion

        #region Private Methods

        private bool IsGameOver()
        {
            return (currentRow.Field1.HintType == HintEnum.RightColorRightPosition &&
                    currentRow.Field2.HintType == HintEnum.RightColorRightPosition &&
                    currentRow.Field3.HintType == HintEnum.RightColorRightPosition &&
                    currentRow.Field4.HintType == HintEnum.RightColorRightPosition);
        }

        private void GetHint(Field headerRowfield, Field currentRowField)
        {
            var headerRow = this.headerRow.First();

            if (headerRowfield.InnerColor == currentRowField.InnerColor)
            {
                currentRowField.HintType = HintEnum.RightColorRightPosition;
            }
            else if (headerRow.Field1.InnerColor == currentRowField.InnerColor ||
                headerRow.Field2.InnerColor == currentRowField.InnerColor ||
                headerRow.Field3.InnerColor == currentRowField.InnerColor ||
                headerRow.Field4.InnerColor == currentRowField.InnerColor)
            {
                currentRowField.HintType = HintEnum.RightColorWrongPosition;
            }
            else
            {
                currentRowField.HintType = HintEnum.WrongColor;
            }
        }

        private StateEnum SetFieldBrush(Field field, Brush color)
        {
            if (DuplicateColor(color))
            {
                return StateEnum.DuplicateColor;
            }

            if (field.State == FieldStateEnum.Empty)
            {
                field.Color = color;
                field.State = FieldStateEnum.Filled;
                
                return StateEnum.FieldFilled;
            }

            return StateEnum.FieldEmpty;
        }

        private bool DuplicateColor(Brush color)
        {
            return this.currentRow.Field1.Color == color ||
                    this.currentRow.Field2.Color == color ||
                    this.currentRow.Field3.Color == color ||
                    this.currentRow.Field4.Color == color;
        }

        private StateEnum RowFilledState()
        {
            if (this.currentRow == null)
            {
                this.GameState = StateEnum.GameOverFailure;
                return StateEnum.GameOverFailure;
            }


            if (IsGameOver())
            {
                this.GameState = StateEnum.GameOverSuccess;
                return StateEnum.GameOverSuccess;
            }

            bool filled = this.currentRow.Field1.State == FieldStateEnum.Filled &&
                this.currentRow.Field2.State == FieldStateEnum.Filled &&
                    this.currentRow.Field3.State == FieldStateEnum.Filled &&
                    this.currentRow.Field4.State == FieldStateEnum.Filled;

            if (filled)
                return StateEnum.RowFilled;
            else
                return StateEnum.FieldEmpty;
        }

        #endregion

    }
}