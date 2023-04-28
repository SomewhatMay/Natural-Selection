namespace Core;

public partial class MainWorld
{
	public static class Statistics {
		private static double updateRate;
		public static double UpdateRate
		{
			get { return updateRate; }
			set
			{
				double oldValue = updateRate;
				updateRate = value;
				OnUpdateRateChanged?.Invoke(new ValueChangedEventArgs<double> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnUpdateRateChanged;

		private static int day;
		public static int Day
		{
			get { return day; }
			set
			{
				int oldValue = day;
				day = value;
				OnDayChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnDayChanged;

		private static int generation;
		public static int Generation
		{
			get { return generation; }
			set
			{
				int oldValue = generation;
				generation = value;
				OnGenerationChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnGenerationChanged;

		private static int foodAlive;
		public static int FoodAlive
		{
			get { return foodAlive; }
			set
			{
				int oldValue = foodAlive;
				foodAlive = value;
				OnFoodAliveChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnFoodAliveChanged;

		private static int foodEaten;
		public static int FoodEaten
		{
			get { return foodEaten; }
			set
			{
				int oldValue = foodEaten;
				foodEaten = value;
				OnFoodEatenChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnFoodEatenChanged;

		private static int cellsAlive;
		public static int CellsAlive
		{
			get { return cellsAlive; }
			set
			{
				int oldValue = cellsAlive;
				cellsAlive = value;
				OnCellsAliveChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnCellsAliveChanged;

		private static int cellsGarrisoned;
		public static int CellsGarrisoned
		{
			get { return cellsGarrisoned; }
			set
			{
				int oldValue = cellsGarrisoned;
				cellsGarrisoned = value;
				OnCellsGarrisonedChanged?.Invoke(new ValueChangedEventArgs<int> { OldValue = oldValue, NewValue = value });
			}
		}
		public static event ChangedHandlerDelegate OnCellsGarrisonedChanged;
	}
}