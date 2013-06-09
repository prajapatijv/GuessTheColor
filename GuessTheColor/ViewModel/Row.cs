using Framework.MVVM;

namespace GuessTheColor
{
    public class Row : ViewModelBase
    {
        public Row NewRow(byte id)
        {
            var row = new Row();

            row.Id = id;

            row.Field1 = Field.NewField(1);
            row.Field2 = Field.NewField(2);
            row.Field3 = Field.NewField(3);
            row.Field4 = Field.NewField(4);

            return row;
        }

        public Row NewHeaderRow()
        {
            var row = new Row();
            row.Id = 0;

            new GameColors().NewGameRandomColors(row);

            return row;
        }


        public byte Id { get; private set; }

        public Field Field1
        {
            get
            {
                return field1;
            }
            set
            {
                field1 = value;
                this.OnPropertyChanged("Field1");
            }
        }

        public Field Field2
        {
            get
            {
                return field2;
            }
            set
            {
                field2 = value;
                this.OnPropertyChanged("Field2");
            }
        }

        public Field Field3
        {
            get
            {
                return field3;
            }
            set
            {
                field3 = value;
                this.OnPropertyChanged("Field3");
            }
        }

        public Field Field4
        {
            get
            {
                return field4;
            }
            set
            {
                field4 = value;
                this.OnPropertyChanged("Field4");
            }
        }

        private Field field1;
        private Field field2;
        private Field field3;
        private Field field4;
    }
}
