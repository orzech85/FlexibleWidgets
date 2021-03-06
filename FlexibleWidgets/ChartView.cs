using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace FlexibleWidgets
{
    public class ChartView : View
    {
        public ChartView(Context context) :
             base(context)
        {
            Initialize();
        }

        public ChartView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public ChartView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            SetBackgroundColor(Color.LightGray);
            _paint.TextSize = Context.Resources.DisplayMetrics.Density * 10;
        }


        public IEnumerable<float> Values { get; set; } = new List<float> {
            3f, 1f, 4.22f, 4.34f, 2f, 5f
        };

        private float _minValue;
        public float MinValue
        {
            get
            {
                if (AutoMin)
                    return Values.Count() > 0 ? Values.Min() : 0;
                else
                    return _minValue;
            }
            set { _minValue = value; }
        }

        private float _maxValue;
        public float MaxValue
        {
            get
            {
                if (AutoMax)
                    return Values.Count() > 0 ? Values.Max() : 0;
                else
                    return _maxValue;
            }
            set { _maxValue = value; }
        }

        public float Range
        {
            get { return (MaxValue - MinValue); }
        }

        public bool AutoMin { get; set; } = true;
        public bool AutoMax { get; set; } = true;

        public string ValueFormat { get; set; } = "F2";

        public Color Color
        {
            get { return _paint.Color; }
            set { _paint.Color = value; }
        }

        public float LineWidth
        {
            get { return _paint.StrokeWidth; }
            set { _paint.StrokeWidth = value; }
        }

        private float _factor;
        private int _labelsCount;
        private float _labelsDistance;
        private float _leftMargin;
        private Paint _paint = new Paint
        {
            StrokeWidth = 5,
        };

        private Paint _paintGrid = new Paint
        {
            StrokeWidth = 1
        };

        Rect chartRect = new Rect();

        protected override void OnDraw(Canvas canvas)
        {
            chartRect.Left = (int)(_paint.MeasureText(MinValue.ToString(ValueFormat)) * 1.2f);
            chartRect.Right = canvas.Width;
            chartRect.Top = (int)(2f * _paint.TextSize);
            chartRect.Bottom = canvas.Height - chartRect.Top;

            _labelsCount = chartRect.Height() / (chartRect.Top * 2);
            _labelsDistance = Range / (_labelsCount);
            _factor = chartRect.Height() / Range;

            canvas.DrawLine(chartRect.Left, chartRect.Top, chartRect.Left, chartRect.Bottom, _paint);

            for (int i = 0; i <= _labelsCount; i++)
            {
                canvas.DrawText((MaxValue - (_labelsDistance) * i).ToString(ValueFormat),
                    0,
                    chartRect.Top + (chartRect.Height() / _labelsCount) * i,
                    _paint
                    );
                canvas.DrawLine(
                    chartRect.Left,
                    chartRect.Top + (chartRect.Height() / _labelsCount) * i,
                    chartRect.Right,
                    chartRect.Top + (chartRect.Height() / _labelsCount) * i,
                    _paintGrid
                    );
            }

            float step = (canvas.Width - chartRect.Left) / (Values.Count() - 1);
            int iterator = 0;

            var lastY = (Values.FirstOrDefault() - MinValue) * _factor + chartRect.Top;

            foreach (var item in Values.Skip(1))
            {
                var newItem = canvas.Height - (item - MinValue) * _factor - chartRect.Top;

                canvas.DrawLine(chartRect.Left + step * iterator, lastY, chartRect.Left + step * (++iterator), newItem, _paint);

                lastY = newItem;
            }
        }
    }
}