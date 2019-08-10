namespace Dash.Forms.Droid
{
    public class Constants : AndroidShared.Constants
    {
        public const float METERS_TO_MILES_DIVISOR = 1609.344f;
        public const float METERS_TO_KILOMETERS_DIVISOR = 1000f;
        public const int LOCATION_SERVICE_RUNNING_NOTIFICATION_ID = 10000;
        public const int LOCATION_MIN_UPDATE_FREQUENCY = 4000;
        public const string SERVICE_STARTED_KEY = "has_service_been_started";
        public const string BROADCAST_MESSAGE_KEY = "broadcast_message";

        public class Action
        {
            public const string LOCATION_CHANGED = "Dash.Action.LOCATION_CHANGED";
            public const string START_SERVICE = "Dash.Action.START_SERVICE";
            public const string STOP_SERVICE = "Dash.Action.STOP_SERVICE";
            public const string RUN_ACTIVITY = "Dash.Action.RUN_ACTIVITY";
            public const string END_RUN = "Dash.Action.END_RUN";
            public const string PAUSE_RUN = "Dash.Action.PAUSE_RUN";
            public const string RESUME_RUN = "Dash.Action.RESUME_RUN";
            public const string SHOW_PAUSE = "Dash.Action.SHOW_PAUSE";
            public const string SHOW_RESUME = "Dash.Action.SHOW_RESUME";
            public const string RUN = "Dash.Action.RUN";
        }

        public class Extra
        {
            public const string LOCATION_DATA = "Dash.Extra.LOCATION_DATA";
            public const string RUN_TRAINING_DATA = "Dash.Extra.RUN_TRAINING_DATA";
            public const string RUN_DATA = "Dash.Extra.RUN_DATA";
        }

        public class Notification
        {
            public const string BROADCAST_ACTION = "Dash.Notification.BROADCAST_ACTION";
        }

        public class Permission
        {
            public const int LOCATION_PERMISSION = 1;
        }

        public class Request
        {
            public const int RUN = 1;
        }
    }
}
