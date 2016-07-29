namespace Teclyn.Core.Commands
{
    public class CommandResultError
    {
        public string Message { get; }
        public string Field { get; }

        public CommandResultError(string message)
        {
            this.Message = message;
        }

        public CommandResultError(string message, string field)
        {
            this.Message = message;
            this.Field = field;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.Field))
            {
                return $"{this.Field} - {this.Message}";
            }

            return this.Message;
        }
    }
}