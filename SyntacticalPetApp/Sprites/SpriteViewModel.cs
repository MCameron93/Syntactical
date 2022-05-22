﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SyntacticalPetApp.Sprites
{
    public class SpriteViewModel : INotifyPropertyChanged
    {
        private readonly Animator animator;

        public SpriteViewModel(Animator animator)
        {
            this.animator = animator ?? throw new ArgumentNullException(nameof(animator));
            animator.FrameChanged += OnFrameChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, Animation> Animations { get; set; }

        public string ImagePath => animator.CurrentFramePath;

        public void PlayAnim()
        {
            animator.Play();
        }

        public void SetAnimation(string animationName)
        {
            if (Animations.TryGetValue(animationName, out var animation))
            {
                animator.SetAnimation(animation);
            }
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
        }
    }
}