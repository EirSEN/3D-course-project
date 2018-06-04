using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity3DCourse.View;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3DCourse
{
    public class PhotoCamera : Weapon
    {
        private AmmoView _ammoView;
        private PhotoCameraView _photoCameraView;
        private Animator _anim;

        private bool _isProcessed;
        private string _path;
        //private int _layers = 5;
        private Camera _camera;

        private void Start()
        {
            _ammoView = FindObjectOfType<AmmoView>();
            _photoCameraView = FindObjectOfType<PhotoCameraView>();
            _anim = GetComponent<Animator>();

            _camera = Camera.main;
            _path = Application.dataPath;
        }

        public override void Fire()
        {

            if (_isProcessed) return;

            _anim.SetTrigger("Photo");
            StartCoroutine(SaveScreenshot());
        }

        public override void AlternativeFire()
        {
            return;
        }

        private IEnumerator SaveScreenshot()
        {
            _isProcessed = true;
            yield return new WaitForSeconds(0.2f);
            // Это костыль, к сожалению. Не разобрался, как убрать оружие\фотоаппарат с экрана в момент скриншота
            IsVisible = false;
            yield return new WaitForEndOfFrame();

            var sw = Screen.width;
            var sh = Screen.height;
            var sc = new Texture2D(sw, sh, TextureFormat.RGB24, false);
            sc.ReadPixels(new Rect(0, 0, sw, sh), 0, 0);
            var bytes = sc.EncodeToPNG();
            var filename = String.Format("{0:ddMMyyyy_HHmmssfff}.png", DateTime.Now);
            IsVisible = true;
            File.WriteAllBytes(_path + filename, bytes);
            yield return null;
            _photoCameraView.PhotoEffect();

            yield return new WaitForSeconds(2.1f);

            _isProcessed = false;
            StopCoroutine(SaveScreenshot());
        }

        public override void SelectWeapon()
        {
            _ammoView.DisplayCurrentAmmo(0);
            _ammoView.DisplayMaxAmmo(0);
        }


        public override void DeselectWeapon()
        {
            return;
        }

        public override void ReloadWeapon()
        {
            return;
        }

    }
}