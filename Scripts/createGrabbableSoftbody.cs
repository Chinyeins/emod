namespace VRTK.Examples
{
    using System;
    using UnityEngine;
    using UnityEventHelper;
    using UnityEngine.SceneManagement;

    public class createGrabbableSoftbody : MonoBehaviour
    {
        public GameObject Prefab;
        public GameObject position;
        public bool useLocation = false;
        public Vector3 Location;
        public GameObject RightController;
        public GameObject LeftController;
        public SliderHandler sliderHandler;
        public FormulaController formulaController;
        public grabThrowable grabHandler;
        public GameObject Menu;
        private VRTK_Button_UnityEvents buttonEvents;

        private void Start()
        {
            try
            {
                buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
                if(buttonEvents == null)
                {
                    buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
                }
                buttonEvents.OnPushed.AddListener(handlePush);
            } catch(Exception e)
            {
                Debug.LogWarning("" + e.Message.ToString());
            }


            //initial reset
            handlePush(new object(), new Control3DEventArgs());
        }

        public void handlePush(object sender, Control3DEventArgs e)
        {
            Transform oldPos = this.position.transform;
            try
            {
                foreach (Transform child in this.position.transform)
                {
                    Destroy(child.gameObject);
                }
                GameObject newSim = Instantiate(Prefab, this.position.transform, false);
                BehaviourModifier mod = newSim.GetComponent<BehaviourModifier>();
                newSim.GetComponent<TxSoftBody>().enabled = false;

                if (useLocation == true)
                {
                    newSim.transform.position = Location;
                } else
                {
                    this.position.transform.position = oldPos.position;
                }

                if (Prefab == null)
                {
                    Debug.LogError("Prefab not found");
                    return;
                }
    
                //pass needed objects to modifier
                
                if (!SceneManager.GetActiveScene().name.Equals("Phys1"))
                {
                    mod.IsSoftbody = true;
                }
                mod.RightController = RightController;
                mod.LeftController = LeftController;
                mod.sliderHandler = LeftController.GetComponent<SliderHandler>();
                mod.formulaController = RightController.GetComponent<FormulaController>();
                mod.grabHandler = RightController.GetComponent<grabThrowable>();

                //modify behaviour based on slider settings
                float[] settings = new float[5];

                settings[0] = 800;
                settings[1] = 0.01643762f;
                settings[2] = UnityEngine.Random.value * 50;
                settings[3] = 1000000;
                settings[4] = 0.08f;

                mod.initSliderSettings(settings);
                mod.fakeWeight = UnityEngine.Random.value * 20;
                mod.updateBehaviour();
            }
            catch (Exception err)
            {
                Debug.LogError("Error restarting sim softbodies \n " + err.Message.ToString() + err.StackTrace.ToString());
            }

            Debug.Log("Hit");
        } 
    }
}