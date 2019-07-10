//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xamarin.Forms;

//namespace Dash.Forms.Controls
//{
//    public class ExpandableFAB : BindableObject
//    {
//        public readonly BindableProperty SubFABsProperty = BindableProperty.Create(nameof(SubFABs), typeof(IEnumerable<FAButton>), typeof(ExpandableFAB),
//            propertyChanged: (b, o, n) => {
//                if (b is ExpandableFAB _this)
//                {

//                }
//            }
//        );
//        public IEnumerable<FAButton> SubFABs
//        {
//            get { return (IEnumerable<FAButton>)GetValue(SubFABsProperty); }
//            set { SetValue(SubFABsProperty, value); }
//        }

//        public ExpandableFAB()
//        {
            
//        }
//    }

//    public class FABConfig
//    {
//        public Color Color { get; set; }
//        public string ImageSource { get; set; }
//        public FABSize Size { get; set; }
//        public Command Command { get; set; }
//    }
//}
