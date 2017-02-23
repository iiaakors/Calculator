using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Infrastructure
{
	public class ColorButton : Button
	{
		public static readonly RoutedEvent CommandExecutedEvent = EventManager.RegisterRoutedEvent(
		"CommandExecuted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorButton));

		// Provide CLR accessors for the event 
		public event RoutedEventHandler CommandExecuted
		{
			add { AddHandler(CommandExecutedEvent, value); }
			remove { RemoveHandler(CommandExecutedEvent, value); }
		}

		// This method raises the Tap event 
		void RaiseCommandExecuted()
		{
			try
			{
				RaiseEvent(new RoutedEventArgs(ColorButton.CommandExecutedEvent));
			}
			catch (Exception ex)
			{ }
		}
		// For demonstration purposes we raise the event when the MyButtonSimple is clicked 

		public ColorButton()
			: base()
		{
			Loaded += new RoutedEventHandler(ColorButton_Loaded);
		}

		void ColorButton_Loaded(object sender, RoutedEventArgs e)
		{
			if (Command != null && Command is IExecuted)
			{
				(Command as IExecuted).CommandExecuted += new EventHandler(ColorButton_CommandExecuted);
			}
		}

		void ColorButton_CommandExecuted(object sender, EventArgs e)
		{
			RaiseCommandExecuted();
		}

		public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(ColorButton));

		public Brush MouseOverForeground
		{
			get { return (Brush)GetValue(MouseOverForegroundProperty); }
			set { SetValue(MouseOverForegroundProperty, value); }
		}
	}
}
