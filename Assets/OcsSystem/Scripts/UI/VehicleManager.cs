using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ocs.GameSystem
{
    public class VehicleManager : MonoBehaviour
    {
        //[SerializeField] private EventSystem eventSystem;

        [SerializeField] private Button WheelLoaderButton;
        [SerializeField] private GameObject WheelLoader;
        private Image WheelLoaderImage;
        [SerializeField] private Button BackHoeButton;
        [SerializeField] private GameObject BackHoe;
        private Image BackHoeImage;
        [SerializeField] private Button TruckButton;
        [SerializeField] private GameObject Truck;
        private Image TruckImage;

        // Start is called before the first frame update
        void Start()
        {
            //firstSelectedButton.Select();
            WheelLoaderImage = WheelLoaderButton.GetComponent<Image>();
            BackHoeImage = BackHoeButton.GetComponent<Image>();
            TruckImage = TruckButton.GetComponent<Image>();

        }

        // Update is called once per frame
        void Update()
        {
            var set = GameSetting.setting;

            if(set.WheelLoader.valid){
                switch_WheelLoader(true);
            }else{
                switch_WheelLoader(false);
            }
            if(set.BackHoe.valid){
                switch_BackHoe(true);
            }else{
                switch_BackHoe(false);
            }
            if(set.Truck.valid){
                switch_Truck(true);
            }else{
                switch_Truck(false);
            }


            /*switch(GameSetting.setting.vehicle)
            {
                case StageSetting.Vehicle.WheelLoader:
                    switch_WheelLoader();
                    break;
                case StageSetting.Vehicle.BackHoe:
                    switch_BackHoe();
                    break;
                case StageSetting.Vehicle.Truck:
                    switch_Truck();
                    break;
                default:
                    break;
            }*/
        }

        public void WheelLoaderClicked()
        {
            if(GameSetting.setting.WheelLoader.valid){
                GameSetting.setting.WheelLoader.valid = false;
            }else{
                GameSetting.setting.WheelLoader.valid = true;
            }
                
        }

        public void BackHoeClicked()
        {
            if(GameSetting.setting.BackHoe.valid){
                GameSetting.setting.BackHoe.valid = false;
            }else{
                GameSetting.setting.BackHoe.valid = true;
            }
            
        }

        public void TruckClicked()
        {
            if(GameSetting.setting.Truck.valid){
                GameSetting.setting.Truck.valid = false;
            }else{
                GameSetting.setting.Truck.valid = true;
            }
            
        }

        private void switch_WheelLoader(bool mode)
        {
            WheelLoaderImage.enabled = mode;
            //とりあえずの実装　あとで直す
            if(!(mode == true && (GameSetting.setting.BackHoe.valid || GameSetting.setting.Truck.valid))){
                WheelLoader.SetActive(mode);
            }
            
        }

        private void switch_BackHoe(bool mode)
        {
            BackHoeImage.enabled = mode;
            //とりあえずの実装
            if(!(mode == true && (GameSetting.setting.WheelLoader.valid || GameSetting.setting.Truck.valid))){
                BackHoe.SetActive (mode);
            }
            
        }

        private void switch_Truck(bool mode)
        {
            TruckImage.enabled = mode;
            if(!(mode == true && (GameSetting.setting.WheelLoader.valid || GameSetting.setting.BackHoe.valid))){
                Truck.SetActive (mode);
            }
            
        }
    }

}