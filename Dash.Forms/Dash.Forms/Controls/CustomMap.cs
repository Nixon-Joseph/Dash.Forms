using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Dash.Forms.Controls
{
    public class CustomMap : Map
    {
        private List<Position> _RouteCoordinates;
        public IEnumerable<Position> RouteCoordinates
        {
            get { return _RouteCoordinates; }
            private set { _RouteCoordinates = value.ToList(); }
        }
        public event EventHandler<NewPositionEventArgs> AddedPosition;

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
        }

        public CustomMap(IEnumerable<Position> positions)
        {
            RouteCoordinates = positions;
        }

        public void AddPosition(Position position, bool showPath = true)
        {
            _RouteCoordinates.Add(position);
            AddedPosition?.Invoke(this, new NewPositionEventArgs(position, showPath));
        }
    }

    public class NewPositionEventArgs : EventArgs
    {
        public Position Position { get; set; }
        public bool ShowPath { get; set; }

        public NewPositionEventArgs(Position position, bool showPath)
        {
            Position = position;
            ShowPath = showPath;
        }
    }
}
