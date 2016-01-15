﻿using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoogleCloudExtension.Controls
{
    /// <summary>
    /// Interaction logic for ProgressIndicator.xaml
    /// </summary>
    public partial class ProgressIndicator : UserControl, ISupportInitialize
    {
        static readonly Lazy<IList<ImageSource>> s_frames = new Lazy<IList<ImageSource>>(LoadAnimationFrames);
        const int FullDuration = 500;
        static readonly Duration s_animationDuration = new Duration(new TimeSpan(0, 0, 0, 0, FullDuration));
        static readonly Lazy<ObjectAnimationUsingKeyFrames> s_animationSource = new Lazy<ObjectAnimationUsingKeyFrames>(CreateAnimation);

        public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register(
            nameof(IsAnimated),
            typeof(bool),
            typeof(ProgressIndicator));

        private Storyboard _storyboard;

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public ProgressIndicator()
        {
            InitializeComponent();
        }

        private static ObjectAnimationUsingKeyFrames CreateAnimation()
        {
            // Initialize the animation for this object.
            var result = new ObjectAnimationUsingKeyFrames
            {
                AutoReverse = true,
                Duration = s_animationDuration,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Creates the frames for the animation.
            var frameDuration = FullDuration / s_frames.Value.Count;
            int framePoint = 0;
            var keyFrames = new ObjectKeyFrameCollection();
            foreach (var frame in s_frames.Value)
            {
                var keyFrame = new DiscreteObjectKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, framePoint),
                    Value = frame,
                };

                framePoint += frameDuration;
            }
            result.KeyFrames = keyFrames;
            result.Freeze();

            return result;
        }

        #region ISupportInitialize

        public override void EndInit()
        {
            base.EndInit();

            if (IsAnimated)
            {
                StartAnimation();
            }
        }

        #endregion

        private void StartAnimation()
        {
            var animation = s_animationSource.Value.Clone();

            _storyboard = new Storyboard();
            _storyboard.Children.Add(animation);

            Storyboard.SetTargetName(animation, "_image");
            Storyboard.SetTargetProperty(animation, new PropertyPath("Source"));

            _storyboard.Begin(this);
        }

        private static List<ImageSource> LoadAnimationFrames()
        {
            return Enumerable.Range(1, 12)
                .Select(x => ResourceUtils.LoadResource($"Controls/Resources/step_{x}.png"))
                .ToList();

        }
    }
}