using MelonLoader;
using RumbleModdingAPI;
using System.Collections;
using UnityEngine;
using Il2CppRUMBLE.Interactions.InteractionBase;
using Il2CppPhoton.Pun;
using Il2CppExitGames.Client.Photon;
using RumbleModUI;

namespace MabelTheCat
{
    public static class ModBuildInfo
    {
        public const string Version = "1.3.0";
    }

    public class main : MelonMod
    {
        System.Random random = new System.Random();
        private GameObject mabel;
        public static GameObject[] spawnedMabels;
        private static List<Transform>[] mabelBones;
        //List<GameObject> holdPoints = new List<GameObject>();
        //public static bool leftHandGrabbed = false;
        //public static bool rightHandGrabbed = false;
        //public static GameObject leftHandHeldJoint, rightHandHeldJoint, leftHand, rightHand;
        //private bool qKeyReleased = true;
        private string currentScene = "Loader";
        private static int sceneCount = 0;
        private bool flatLandFound = false;
        private static bool flatLandPressed = false;
        private bool inFlatLand = false;
        private bool init = false;
        public static event Action mabelPosingReady;
        private int customMapSceneCount = 0;
        public static Mod MabelTheCat = new Mod();
        private bool showAllCats = false;
        private static GameObject mabelParent;

        public GameObject LoadAssetBundle(string bundleName, string objectName)
        {
            using (Stream bundleStream = MelonAssembly.Assembly.GetManifestResourceStream(bundleName))
            {
                byte[] bundleBytes = new byte[bundleStream.Length];
                bundleStream.Read(bundleBytes, 0, bundleBytes.Length);
                Il2CppAssetBundle bundle = Il2CppAssetBundleManager.LoadFromMemory(bundleBytes);
                return UnityEngine.Object.Instantiate(bundle.LoadAsset<GameObject>(objectName));
            }
        }

        public override void OnLateInitializeMelon()
        {
            Calls.onMyModsGathered += checkMods;
            Calls.onMapInitialized += mapInit;
            mabel = LoadAssetBundle("MabelTheCat.mabel", "cat");
            mabel.name = "Mabel";
            GameObject.DontDestroyOnLoad(mabel);
            mabel.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            //ReskinCat();
            mabel.SetActive(false);
            MabelTheCat.ModName = "MabelTheCat";
            MabelTheCat.ModVersion = ModBuildInfo.Version;
            MabelTheCat.SetFolder("MabelTheCat");
            MabelTheCat.AddToList("Show all Cats", false, 0, "Toggling ON will have all the Cats Shown.", new Tags { });
            MabelTheCat.GetFromFile();
            MabelTheCat.ModSaved += Save;
            UI.instance.UI_Initialized += UIInit;
            showAllCats = (bool)MabelTheCat.Settings[0].SavedValue;
        }

        private void UIInit()
        {
            UI.instance.AddMod(MabelTheCat);
        }

        private void Save()
        {
            if (showAllCats != (bool)MabelTheCat.Settings[0].SavedValue)
            {
                showAllCats = (bool)MabelTheCat.Settings[0].SavedValue;
                removeMabels();
                if (inFlatLand)
                {
                    createMabels(true);
                }
                else
                {
                    createMabels(false);
                }
                poseCats();
            }
        }

        private void checkMods()
        {
            flatLandFound = Calls.Mods.findOwnMod("FlatLand", "1.6.0", false);
        }
        public void OnEvent(EventData eventData)
        {
            if (eventData.Code == 70)
            {
                customMapSceneCount = sceneCount;
                removeMabels();
            }
        }

        private void removeMabels()
        {
            GameObject.DestroyImmediate(mabelParent);
        }

        private void poseCats()
        {
            if (currentScene == "Gym")
            {
                if (flatLandFound)
                {
                    MelonCoroutines.Start(listenForFlatLandButton());
                }
                if (showAllCats)
                {
                    PoseLayingDown(0);
                    spawnedMabels[0].transform.position = new Vector3(9.9f, 0.8573f, -0.6227f);
                    spawnedMabels[0].transform.localRotation = Quaternion.Euler(306.764f, 319.4213f, 13.756f);
                    StartTailWagStanding(1);
                    spawnedMabels[1].transform.position = new Vector3(5.0155f, 0.35f, 1.6736f);
                    spawnedMabels[1].transform.localRotation = Quaternion.Euler(0f, 206.7709f, 0f);
                    PoseSleeping(2);
                    spawnedMabels[2].transform.position = new Vector3(4.3618164f, 1.5500001f, 9.430909f);
                    spawnedMabels[2].transform.localRotation = Quaternion.Euler(347.7088f, 146.4092f, 95.1773f);
                    PoseSleeping(3);
                    spawnedMabels[3].transform.position = new Vector3(-4.0618153f, 0.20091006f, -1.5218217f);
                    spawnedMabels[3].transform.localRotation = Quaternion.Euler(347.7088f, 146.4092f, 95.17729f);
                    PoseSitting(4);
                    spawnedMabels[4].transform.position = new Vector3(-3.61f, 5.1291f, 2.4427f);
                    spawnedMabels[4].transform.localRotation = Quaternion.Euler(306.0583f, 130.39539f, -8.122779E-05f);
                    PoseStanding(5);
                    spawnedMabels[5].transform.position = new Vector3(4.0682f, 2.8709f, 9.8809f);
                    spawnedMabels[5].transform.localRotation = Quaternion.Euler(-0f, 206.7709f, 0f);
                }
                else
                {
                    int pose = random.Next(0, 6);
                    switch (pose)
                    {
                        case 0:
                            PoseLayingDown(0);
                            spawnedMabels[0].transform.position = new Vector3(9.9f, 0.8573f, -0.6227f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(306.764f, 319.4213f, 13.756f);
                            break;
                        case 1:
                            StartTailWagStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(5.0155f, 0.35f, 1.6736f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 206.7709f, 0f);
                            break;
                        case 2:
                            PoseSleeping(0);
                            spawnedMabels[0].transform.position = new Vector3(4.3618164f, 1.5500001f, 9.430909f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(347.7088f, 146.4092f, 95.1773f);
                            break;
                        case 3:
                            PoseSleeping(0);
                            spawnedMabels[0].transform.position = new Vector3(-4.0618153f, 0.20091006f, -1.5218217f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(347.7088f, 146.4092f, 95.17729f);
                            break;
                        case 4:
                            PoseSitting(0);
                            spawnedMabels[0].transform.position = new Vector3(-3.61f, 5.1291f, 2.4427f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(306.0583f, 130.39539f, -8.122779E-05f);
                            break;
                        case 5:
                            PoseStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(4.0682f, 2.8709f, 9.8809f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(-0f, 206.7709f, 0f);
                            break;
                    }
                }
            }
            else if (currentScene == "Park")
            {
                if (showAllCats)
                {
                    StartTailWagStanding(0);
                    spawnedMabels[0].transform.position = new Vector3(-15.2646f, -3.2428f, -9.9838f);
                    spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 234.0795f, 0f);
                    PoseSitting(1);
                    spawnedMabels[1].transform.position = new Vector3(-24.8039f, 0.8077f, -16.2777f);
                    spawnedMabels[1].transform.localRotation = Quaternion.Euler(322.0467f, 64.7633f, 0f);
                    PoseStanding(2);
                    spawnedMabels[2].transform.position = new Vector3(14.2072f, 6.9445f, 6.0545f);
                    spawnedMabels[2].transform.localRotation = Quaternion.Euler(0f, 281.874f, 0f);
                    PoseSleeping(3);
                    spawnedMabels[3].transform.position = new Vector3(-22.4429f, -5.9974f, 0.6699f);
                    spawnedMabels[3].transform.localRotation = Quaternion.Euler(0f, 62.9994f, 92.1347f);
                    PoseLayingDown(4);
                    spawnedMabels[4].transform.position = new Vector3(5.8098f, -0.1547f, 5.3263f);
                    spawnedMabels[4].transform.localRotation = Quaternion.Euler(302.1827f, 232.8239f, 0f);
                    PoseSitting(5);
                    spawnedMabels[5].transform.position = new Vector3(-25.9657f, 14.6567f, 12.3286f);
                    spawnedMabels[5].transform.localRotation = Quaternion.Euler(308.2613f, 158.6978f, 0f);
                }
                else
                {
                    int pose = random.Next(0, 6);
                    switch (pose)
                    {
                        case 0:
                            StartTailWagStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(-15.2646f, -3.2428f, -9.9838f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 234.0795f, 0f);
                            break;
                        case 1:
                            PoseSitting(0);
                            spawnedMabels[0].transform.position = new Vector3(-24.8039f, 0.8077f, -16.2777f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(322.0467f, 64.7633f, 0f);
                            break;
                        case 2:
                            PoseStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(14.2072f, 6.9445f, 6.0545f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 281.874f, 0f);
                            break;
                        case 3:
                            PoseSleeping(0);
                            spawnedMabels[0].transform.position = new Vector3(-22.4429f, -5.9974f, 0.6699f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 62.9994f, 92.1347f);
                            break;
                        case 4:
                            PoseLayingDown(0);
                            spawnedMabels[0].transform.position = new Vector3(5.8098f, -0.1547f, 5.3263f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(302.1827f, 232.8239f, 0f);
                            break;
                        case 5:
                            PoseSitting(0);
                            spawnedMabels[0].transform.position = new Vector3(-25.9657f, 14.6567f, 12.3286f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(308.2613f, 158.6978f, 0f);
                            break;
                    }
                }
            }
            else if (currentScene == "Map0")
            {
                if (showAllCats)
                {
                    PoseSitting(0);
                    spawnedMabels[0].transform.position = new Vector3(-8.0657f, 3.3299f, 19.8217f);
                    spawnedMabels[0].transform.localRotation = Quaternion.Euler(295.1778f, 162.5442f, 0f);
                    PoseStanding(1);
                    spawnedMabels[1].transform.position = new Vector3(-19.352f, 2.3619f, -9.9329f);
                    spawnedMabels[1].transform.localRotation = Quaternion.Euler(0f, 114.5284f, 0f);
                    PoseSleeping(2);
                    spawnedMabels[2].transform.position = new Vector3(16.0696f, 2.5119f, -13.5693f);
                    spawnedMabels[2].transform.localRotation = Quaternion.Euler(0f, 0f, 101.1101f);
                    PoseLayingDown(3);
                    spawnedMabels[3].transform.position = new Vector3(19.4568f, 2.7795f, 3.156f);
                    spawnedMabels[3].transform.localRotation = Quaternion.Euler(298.0505f, 256.9204f, 0f);
                }
                else
                {
                    int pose = random.Next(0, 4);
                    switch (pose)
                    {
                        case 0:
                            PoseSitting(0);
                            spawnedMabels[0].transform.position = new Vector3(-8.0657f, 3.3299f, 19.8217f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(295.1778f, 162.5442f, 0f);
                            break;
                        case 1:
                            PoseStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(-19.352f, 2.3619f, -9.9329f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 114.5284f, 0f);
                            break;
                        case 2:
                            PoseSleeping(0);
                            spawnedMabels[0].transform.position = new Vector3(16.0696f, 2.5119f, -13.5693f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 0f, 101.1101f);
                            break;
                        case 3:
                            PoseLayingDown(0);
                            spawnedMabels[0].transform.position = new Vector3(19.4568f, 2.7795f, 3.156f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(298.0505f, 256.9204f, 0f);
                            break;
                    }
                }
            }
            else if (currentScene == "Map1")
            {
                if (showAllCats)
                {
                    PoseSitting(0);
                    spawnedMabels[0].transform.position = new Vector3(14.1713f, 10.4217f, -6.5722f);
                    spawnedMabels[0].transform.localRotation = Quaternion.Euler(305.0462f, 300.4279f, 0f);
                    PoseStanding(1);
                    spawnedMabels[1].transform.position = new Vector3(-5.8972f, 7.4398f, -11.3046f);
                    spawnedMabels[1].transform.localRotation = Quaternion.Euler(10.3407f, 80.0742f, 8.0421f);
                    PoseSleeping(2);
                    spawnedMabels[2].transform.position = new Vector3(2.1673f, 5.0421f, 11.3458f);
                    spawnedMabels[2].transform.localRotation = Quaternion.Euler(0f, 187.7051f, 90f);
                    PoseLayingDown(3);
                    spawnedMabels[3].transform.position = new Vector3(-13.8633f, 7.94f, -5.8903f);
                    spawnedMabels[3].transform.localRotation = Quaternion.Euler(323.3444f, 51.1345f, 0f);
                }
                else
                {
                    int pose = random.Next(0, 4);
                    switch (pose)
                    {
                        case 0:
                            PoseSitting(0);
                            spawnedMabels[0].transform.position = new Vector3(14.1713f, 10.4217f, -6.5722f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(305.0462f, 300.4279f, 0f);
                            break;
                        case 1:
                            PoseStanding(0);
                            spawnedMabels[0].transform.position = new Vector3(-5.8972f, 7.4398f, -11.3046f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(10.3407f, 80.0742f, 8.0421f);
                            break;
                        case 2:
                            PoseSleeping(0);
                            spawnedMabels[0].transform.position = new Vector3(2.1673f, 5.0421f, 11.3458f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 187.7051f, 90f);
                            break;
                        case 3:
                            PoseLayingDown(0);
                            spawnedMabels[0].transform.position = new Vector3(-13.8633f, 7.94f, -5.8903f);
                            spawnedMabels[0].transform.localRotation = Quaternion.Euler(323.3444f, 51.1345f, 0f);
                            break;
                    }
                }
            }
            for (int i = 0; i < spawnedMabels.Length; i++)
            {
                spawnedMabels[i].SetActive(true);
            }
        }

        private void mapInit()
        {
            if (!init)
            {
                PhotonNetwork.NetworkingClient.EventReceived += (Action<EventData>)OnEvent;
                init = true;
            }
            createMabels(false);
            poseCats();
            mabelPosingReady?.Invoke();
            if (customMapSceneCount != sceneCount)
            {
                for (int i = 0; i < spawnedMabels.Length; i++)
                {
                    spawnedMabels[i].SetActive(true);
                }
            }
        }

        private void createMabels(bool isFlatLand)
        {
            mabelParent = new GameObject("Mabels");
            if (showAllCats)
            {
                if (isFlatLand)
                {
                    spawnedMabels = new GameObject[1];
                    mabelBones = new List<Transform>[1];
                }
                else if ((currentScene == "Gym") || (currentScene == "Park"))
                {
                    spawnedMabels = new GameObject[6];
                    mabelBones = new List<Transform>[6];
                }
                else if ((currentScene == "Map0") || (currentScene == "Map1"))
                {
                    spawnedMabels = new GameObject[4];
                    mabelBones = new List<Transform>[4];
                }
            }
            else
            {
                spawnedMabels = new GameObject[1];
                mabelBones = new List<Transform>[1];
            }
            for (int i = 0; i <mabelBones.Length; i++)
            {
                mabelBones[i] = new List<Transform>();
            }
            for (int i = 0;i < spawnedMabels.Length; i++)
            {
                spawnedMabels[i] = GameObject.Instantiate(mabel);
                spawnedMabels[i].name = "Mabel";
                spawnedMabels[i].transform.GetChild(0).gameObject.name = "Mesh";
                spawnedMabels[i].transform.parent = mabelParent.transform;
                mabelBones[i].Clear();
                GameObject mabelSpine = spawnedMabels[i].transform.GetChild(1).GetChild(0).gameObject;
                mabelBones[i].Add(mabelSpine.transform.GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(1));
                mabelBones[i].Add(mabelSpine.transform.GetChild(1).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(1).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(2));
                mabelBones[i].Add(mabelSpine.transform.GetChild(2).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(2).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(2));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(2).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(3));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(3).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(4));
                mabelBones[i].Add(mabelSpine.transform.GetChild(4).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(4).GetChild(0).GetChild(0));
                mabelBones[i].Add(mabelSpine.transform.GetChild(4).GetChild(0).GetChild(0).GetChild(0));
            }
        }

        private IEnumerator listenForFlatLandButton()
        {
            yield return new WaitForSeconds(1);
            GameObject.Find("FlatLand/FlatLandButton/Button").GetComponent<InteractionButton>().onPressed.AddListener(new Action(() =>
            {
                flatLandPressed = true;
                MelonCoroutines.Start(toFlatLand());
            }));
            yield break;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            currentScene = sceneName;
            sceneCount++;
            inFlatLand = false;
        }

        private IEnumerator toFlatLand()
        {
            inFlatLand = true;
            yield return new WaitForSeconds(1f);
            flatLandPressed = false;
            removeMabels();
            createMabels(true);
            StartTailWagStanding(0);
            spawnedMabels[0].transform.position = new Vector3(5.0155f, 0.35f, 1.6736f);
            spawnedMabels[0].transform.localRotation = Quaternion.Euler(0f, 206.7709f, 0f);
            spawnedMabels[0].SetActive(true);
        }

        public static IEnumerator JointWag(Transform joint, float[] minMax, float speed, int axis, float waitTime)
        {
            int currentSceneCount = sceneCount;
            while ((joint != null) && (currentSceneCount == sceneCount) && !flatLandPressed && (mabelParent != null))
            {
                float jointRotation;
                try
                {
                    switch (axis)
                    {
                        case 0:
                            joint.localRotation = Quaternion.Euler(joint.localRotation.eulerAngles.x + speed, joint.localRotation.eulerAngles.y, joint.localRotation.eulerAngles.z);
                            jointRotation = joint.localRotation.eulerAngles.x;
                            break;
                        case 1:
                            joint.localRotation = Quaternion.Euler(joint.localRotation.eulerAngles.x, joint.localRotation.eulerAngles.y + speed, joint.localRotation.eulerAngles.z);
                            jointRotation = joint.localRotation.eulerAngles.y;
                            break;
                        case 2:
                            joint.localRotation = Quaternion.Euler(joint.localRotation.eulerAngles.x, joint.localRotation.eulerAngles.y, joint.localRotation.eulerAngles.z + speed);
                            jointRotation = joint.localRotation.eulerAngles.z;
                            break;
                        default:
                            jointRotation = 0;
                            break;
                    }
                }
                catch
                {
                    yield break;
                }
                if (speed < 0)
                {
                    if ((minMax[0] < 0) && (jointRotation <= minMax[0] + 360) && (jointRotation > 300))
                    {
                        speed *= -1;
                        if (waitTime > 0)
                        {
                            yield return new WaitForSeconds(waitTime);
                        }
                    }
                    else if ((minMax[0] > 0) && (jointRotation <= minMax[0]))
                    {
                        speed *= -1;
                        if (waitTime > 0)
                        {
                            yield return new WaitForSeconds(waitTime);
                        }
                    }
                }
                else if ((jointRotation >= minMax[1]) && (jointRotation < 300))
                {
                    speed *= -1;
                }
                yield return new WaitForFixedUpdate();
            }
            yield break;
        }

        private void PoseLayingDown(int mabelToTransform)
        {
            mabelBones[mabelToTransform][0].localRotation = Quaternion.Euler(64.954185f, 15.453509f, 26.672903f);
            mabelBones[mabelToTransform][1].localRotation = Quaternion.Euler(3.1209786f, 4.4456854f, 56.365433f);
            mabelBones[mabelToTransform][2].localRotation = Quaternion.Euler(357.38712f, 157.48375f, 54.1958f);
            mabelBones[mabelToTransform][3].localRotation = Quaternion.Euler(323.17166f, 184.83246f, 179.21472f);
            mabelBones[mabelToTransform][4].localRotation = Quaternion.Euler(39.195377f, 233.07953f, 243.2471f);
            mabelBones[mabelToTransform][5].localRotation = Quaternion.Euler(334.6968f, 13.72322f, 9.599679f);
            mabelBones[mabelToTransform][6].localRotation = Quaternion.Euler(3.1209688f, 355.5546f, 303.63458f);
            mabelBones[mabelToTransform][7].localRotation = Quaternion.Euler(359.37982f, 208.90948f, 305.92798f);
            mabelBones[mabelToTransform][8].localRotation = Quaternion.Euler(327.33673f, 185.46567f, 176.45235f);
            mabelBones[mabelToTransform][9].localRotation = Quaternion.Euler(44.73943f, 235.17944f, 213.96765f);
            mabelBones[mabelToTransform][10].localRotation = Quaternion.Euler(337.84064f, 353.22662f, 342.9142f);
            mabelBones[mabelToTransform][11].localRotation = Quaternion.Euler(29.401266f, 356.84106f, 355.70065f);
            mabelBones[mabelToTransform][12].localRotation = Quaternion.Euler(21.440294f, 6.2652574f, 356.9603f);
            mabelBones[mabelToTransform][13].localRotation = Quaternion.Euler(87.97078f, 180.02342f, 180.0234f);
            mabelBones[mabelToTransform][14].localRotation = Quaternion.Euler(315.02142f, -9.655743E-05f, 6.825087E-05f);
            mabelBones[mabelToTransform][15].localRotation = Quaternion.Euler(58.859253f, 179.99841f, 179.99858f);
            mabelBones[mabelToTransform][16].localRotation = Quaternion.Euler(289.66806f, 306.76492f, 201.24818f);
            mabelBones[mabelToTransform][17].localRotation = Quaternion.Euler(52.753235f, 359.4264f, 29.824442f);
            mabelBones[mabelToTransform][18].localRotation = Quaternion.Euler(5.628575f, 22.14081f, 353.35345f);
            mabelBones[mabelToTransform][19].localRotation = Quaternion.Euler(345.66638f, 343.97522f, 359.7445f);
            mabelBones[mabelToTransform][20].localRotation = Quaternion.Euler(289.6681f, 53.235184f, 158.75185f);
            mabelBones[mabelToTransform][21].localRotation = Quaternion.Euler(29.813198f, 337.28467f, 309.2062f);
            mabelBones[mabelToTransform][22].localRotation = Quaternion.Euler(23.293386f, 14.090822f, 358.94537f);
            mabelBones[mabelToTransform][23].localRotation = Quaternion.Euler(323.03394f, 348.47235f, 347.2814f);
            mabelBones[mabelToTransform][24].localRotation = Quaternion.Euler(357.63348f, 4.0957664E-05f, 1.1290936E-06f);
            mabelBones[mabelToTransform][25].localRotation = Quaternion.Euler(8.128601f, -2.7718724E-07f, -3.9022475E-06f);
            mabelBones[mabelToTransform][26].localRotation = Quaternion.Euler(359.11588f, 1.4551167E-05f, 5.4072797E-07f);
            mabelBones[mabelToTransform][27].localRotation = Quaternion.Euler(294.96143f, -1.8863439E-05f, 2.8531915E-05f);
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(270.10086f, 260.37982f, 0f);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(61.45423f, 62.812504f, 11.801867f);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(17.759802f, 2.9351134E-15f, -1.2222483E-17f);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(4.598563f, 2.2471763E-17f, -2.2091666E-18f);
            StartTailWagLaying(mabelToTransform);
        }

        private void PoseSleeping(int mabelToTransform)
        {
            mabelBones[mabelToTransform][0].localRotation = Quaternion.Euler(333.25577f, 129.47911f, 11.290493f);
            mabelBones[mabelToTransform][1].localRotation = Quaternion.Euler(3.1209786f, 4.4456854f, 56.365433f);
            mabelBones[mabelToTransform][2].localRotation = Quaternion.Euler(339.37158f, 148.01077f, 50.255894f);
            mabelBones[mabelToTransform][3].localRotation = Quaternion.Euler(325.35873f, 219.4933f, 149.05473f);
            mabelBones[mabelToTransform][4].localRotation = Quaternion.Euler(73.65269f, 255.12549f, 219.2606f);
            mabelBones[mabelToTransform][5].localRotation = Quaternion.Euler(336.0943f, 18.059381f, 15.658356f);
            mabelBones[mabelToTransform][6].localRotation = Quaternion.Euler(3.1209688f, 355.5546f, 303.63458f);
            mabelBones[mabelToTransform][7].localRotation = Quaternion.Euler(336.07742f, 206.04381f, 310.56415f);
            mabelBones[mabelToTransform][8].localRotation = Quaternion.Euler(303.69308f, 203.37262f, 166.95901f);
            mabelBones[mabelToTransform][9].localRotation = Quaternion.Euler(71.686005f, 105.708694f, 95.61362f);
            mabelBones[mabelToTransform][10].localRotation = Quaternion.Euler(340.04678f, 2.958346f, 349.5857f);
            mabelBones[mabelToTransform][11].localRotation = Quaternion.Euler(51.690998f, 3.105255f, 10.32038f);
            mabelBones[mabelToTransform][12].localRotation = Quaternion.Euler(35.464985f, 357.97955f, 356.40466f);
            mabelBones[mabelToTransform][13].localRotation = Quaternion.Euler(87.97078f, 180.02342f, 180.0234f);
            mabelBones[mabelToTransform][14].localRotation = Quaternion.Euler(351.70013f, 4.1896443f, 4.747881f);
            mabelBones[mabelToTransform][15].localRotation = Quaternion.Euler(58.859253f, 179.99841f, 179.99858f);
            mabelBones[mabelToTransform][16].localRotation = Quaternion.Euler(358.4639f, 208.5479f, 325.71515f);
            mabelBones[mabelToTransform][17].localRotation = Quaternion.Euler(318.0152f, 162.2435f, 143.24417f);
            mabelBones[mabelToTransform][18].localRotation = Quaternion.Euler(23.63959f, -4.9624312E-05f, -0.0001852999f);
            mabelBones[mabelToTransform][19].localRotation = Quaternion.Euler(15.683508f, 347.6637f, 353.93878f);
            mabelBones[mabelToTransform][20].localRotation = Quaternion.Euler(332.47003f, 144.3116f, 51.28744f);
            mabelBones[mabelToTransform][21].localRotation = Quaternion.Euler(302.77478f, 155.6745f, 222.43324f);
            mabelBones[mabelToTransform][22].localRotation = Quaternion.Euler(84.393074f, 37.8366f, 26.441755f);
            mabelBones[mabelToTransform][23].localRotation = Quaternion.Euler(320.5011f, 15.11045f, 348.53348f);
            mabelBones[mabelToTransform][24].localRotation = Quaternion.Euler(35.436886f, 10.085943f, 359.66937f);
            mabelBones[mabelToTransform][25].localRotation = Quaternion.Euler(8.128601f, -2.7718724E-07f, -3.9022475E-06f);
            mabelBones[mabelToTransform][26].localRotation = Quaternion.Euler(359.11588f, 1.4551167E-05f, 5.4072797E-07f);
            mabelBones[mabelToTransform][27].localRotation = Quaternion.Euler(343.39624f, 342.43906f, 352.28305f);
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(45.22453f, 180.6039f, 166.03857f);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(359.1182f, 87.17388f, 317.72626f);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(0.2796675f, 337.10666f, 306.12775f);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(330.37283f, 329.04764f, 328.39276f);
        }

        private void PoseSitting(int mabelToTransform)
        {
            mabelBones[mabelToTransform][0].localRotation = Quaternion.Euler(60.704338f, 335.02463f, 341.34723f);
            mabelBones[mabelToTransform][1].localRotation = Quaternion.Euler(3.1209786f, 4.4456854f, 56.365433f);
            mabelBones[mabelToTransform][2].localRotation = Quaternion.Euler(7.8021617f, 164.9378f, 44.55482f);
            mabelBones[mabelToTransform][3].localRotation = Quaternion.Euler(315.27023f, 193.11171f, 173.50316f);
            mabelBones[mabelToTransform][4].localRotation = Quaternion.Euler(77.25561f, 323.94925f, 329.14777f);
            mabelBones[mabelToTransform][5].localRotation = Quaternion.Euler(335.40265f, 359.23508f, 6.788837f);
            mabelBones[mabelToTransform][6].localRotation = Quaternion.Euler(3.120968f, 355.5546f, 303.63458f);
            mabelBones[mabelToTransform][7].localRotation = Quaternion.Euler(352.818f, 191.96672f, 317.15573f);
            mabelBones[mabelToTransform][8].localRotation = Quaternion.Euler(314.94208f, 217.37848f, 160.25224f);
            mabelBones[mabelToTransform][9].localRotation = Quaternion.Euler(75.161964f, 121.47308f, 74.16353f);
            mabelBones[mabelToTransform][10].localRotation = Quaternion.Euler(322.0849f, 357.29453f, 342.4746f);
            mabelBones[mabelToTransform][11].localRotation = Quaternion.Euler(18.673882f, 349.94006f, 4.8356986f);
            mabelBones[mabelToTransform][12].localRotation = Quaternion.Euler(7.66934f, 3.4410949f, 356.7834f);
            mabelBones[mabelToTransform][13].localRotation = Quaternion.Euler(87.97059f, 180.02342f, 180.0234f);
            mabelBones[mabelToTransform][14].localRotation = Quaternion.Euler(340.26074f, -9.6557764E-05f, 6.8251065E-05f);
            mabelBones[mabelToTransform][15].localRotation = Quaternion.Euler(58.859257f, 179.99841f, 179.99858f);
            mabelBones[mabelToTransform][16].localRotation = Quaternion.Euler(289.6681f, 306.76492f, 201.24818f);
            mabelBones[mabelToTransform][17].localRotation = Quaternion.Euler(19.499407f, 334.3408f, 26.786076f);
            mabelBones[mabelToTransform][18].localRotation = Quaternion.Euler(23.639585f, -4.9624312E-05f, -0.00018529988f);
            mabelBones[mabelToTransform][19].localRotation = Quaternion.Euler(7.575464f, 13.0273485f, 349.63495f);
            mabelBones[mabelToTransform][20].localRotation = Quaternion.Euler(289.66812f, 53.23518f, 158.75185f);
            mabelBones[mabelToTransform][21].localRotation = Quaternion.Euler(23.990896f, 34.429726f, 333.46115f);
            mabelBones[mabelToTransform][22].localRotation = Quaternion.Euler(23.63958f, 5.61523E-05f, 0.00019950987f);
            mabelBones[mabelToTransform][23].localRotation = Quaternion.Euler(5.360162f, 358.5285f, 3.332931f);
            mabelBones[mabelToTransform][24].localRotation = Quaternion.Euler(357.63348f, 4.0957664E-05f, 1.1290934E-06f);
            mabelBones[mabelToTransform][25].localRotation = Quaternion.Euler(37.79036f, 358.7982f, 12.7111025f);
            mabelBones[mabelToTransform][26].localRotation = Quaternion.Euler(16.774218f, 359.1383f, 354.23416f);
            mabelBones[mabelToTransform][27].localRotation = Quaternion.Euler(294.96143f, -1.886344E-05f, 2.8531918E-05f);
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(304.89563f, 126.84589f, 213.60846f);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(23.653973f, 1.5278426f, 15.393756f);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(34.795444f, 332.2278f, 0.42652133f);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(28.697893f, 346.5803f, 349.73682f);
            StartTailWagSitting(mabelToTransform);
        }

        private void PoseStanding(int mabelToTransform)
        {
            mabelBones[mabelToTransform][0].localRotation = Quaternion.Euler(85.23194f, -1.4076705E-05f, -0.00016377385f);
            mabelBones[mabelToTransform][1].localRotation = Quaternion.Euler(3.1209786f, 4.4456854f, 56.365433f);
            mabelBones[mabelToTransform][2].localRotation = Quaternion.Euler(326.57452f, 95.50591f, 82.301f);
            mabelBones[mabelToTransform][3].localRotation = Quaternion.Euler(309.5199f, 359.71915f, 0.3281641f);
            mabelBones[mabelToTransform][4].localRotation = Quaternion.Euler(54.69404f, -0.00030330702f, -0.00058570225f);
            mabelBones[mabelToTransform][5].localRotation = Quaternion.Euler(47.206306f, -0.00019578602f, -0.0004480658f);
            mabelBones[mabelToTransform][6].localRotation = Quaternion.Euler(3.1209688f, 355.5546f, 303.63458f);
            mabelBones[mabelToTransform][7].localRotation = Quaternion.Euler(326.57452f, 264.4941f, 277.69897f);
            mabelBones[mabelToTransform][8].localRotation = Quaternion.Euler(309.5199f, 0.28084344f, 359.6718f);
            mabelBones[mabelToTransform][9].localRotation = Quaternion.Euler(54.694065f, 0.00031329607f, 0.00060501765f);
            mabelBones[mabelToTransform][10].localRotation = Quaternion.Euler(47.206306f, 0.00020223352f, 0.000462821f);
            mabelBones[mabelToTransform][11].localRotation = Quaternion.Euler(6.1483536f, 0.00015026508f, -8.189325E-11f);
            mabelBones[mabelToTransform][12].localRotation = Quaternion.Euler(355.7813f, 7.81769E-10f, -1.6325444E-12f);
            mabelBones[mabelToTransform][13].localRotation = Quaternion.Euler(87.97078f, 180.02342f, 180.0234f);
            mabelBones[mabelToTransform][14].localRotation = Quaternion.Euler(315.02142f, -9.655743E-05f, 6.825087E-05f);
            mabelBones[mabelToTransform][15].localRotation = Quaternion.Euler(84.74238f, -0.00017562282f, 2.4879216E-09f);
            mabelBones[mabelToTransform][16].localRotation = Quaternion.Euler(289.66806f, 306.76492f, 201.24818f);
            mabelBones[mabelToTransform][17].localRotation = Quaternion.Euler(326.8363f, 320.70926f, 35.50474f);
            mabelBones[mabelToTransform][18].localRotation = Quaternion.Euler(23.63959f, -4.9624312E-05f, -0.0001852999f);
            mabelBones[mabelToTransform][19].localRotation = Quaternion.Euler(43.84563f, -0.00016035534f, -0.00039843447f);
            mabelBones[mabelToTransform][20].localRotation = Quaternion.Euler(289.6681f, 53.235184f, 158.75185f);
            mabelBones[mabelToTransform][21].localRotation = Quaternion.Euler(326.8364f, 39.29078f, 324.49524f);
            mabelBones[mabelToTransform][22].localRotation = Quaternion.Euler(23.639582f, 5.61523E-05f, 0.00019950987f);
            mabelBones[mabelToTransform][23].localRotation = Quaternion.Euler(43.84563f, 0.00015725625f, 0.00042467614f);
            mabelBones[mabelToTransform][24].localRotation = Quaternion.Euler(357.63348f, 4.0957664E-05f, 1.1290936E-06f);
            mabelBones[mabelToTransform][25].localRotation = Quaternion.Euler(8.128601f, -2.7718724E-07f, -3.9022475E-06f);
            mabelBones[mabelToTransform][26].localRotation = Quaternion.Euler(37.642838f, 348.62793f, 43.37256f);
            mabelBones[mabelToTransform][27].localRotation = Quaternion.Euler(294.5568f, -1.886352E-05f, 2.8532124E-05f);
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(15.681303f, 180.00014f, 180f);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(6.44504f, 8.069696E-10f, -6.5554927E-12f);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(7.599642f, 2.935112E-15f, -1.22223165E-17f);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(4.598563f, 2.2471763E-17f, -2.2091666E-18f);
            StartTailWagStanding(mabelToTransform);
        }

        public static void StartTailWagLaying(int mabelToTransform)
        {
            float[] tailWagRotationMaxes = new float[2] { 1, 30 };
            float tailWagSpeed = 1;
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][31], tailWagRotationMaxes, tailWagSpeed, 2, 1));
        }

        public static void StartTailWagStanding(int mabelToTransform)
        {
            float[][] tailWagRotationMaxes = new float[4][];
            tailWagRotationMaxes[0] = new float[2] { 145, 215 };
            tailWagRotationMaxes[1] = new float[2] { -35, 35 };
            tailWagRotationMaxes[2] = new float[2] { -35, 35 };
            tailWagRotationMaxes[3] = new float[2] { -35, 35 };
            float[] tailStartingRotation = new float[4] { 180, 330, 350, 350 };
            float[] tailWagSpeed = new float[4] { 2f, 2f, -2f, -2f };
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(15.6784f, 180f, tailStartingRotation[0]);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(6.445f, 0f, tailStartingRotation[1]);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(7.5996f, 0f, tailStartingRotation[2]);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(4.5986f, 0f, tailStartingRotation[3]);
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][28], tailWagRotationMaxes[0], tailWagSpeed[0], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][29], tailWagRotationMaxes[1], tailWagSpeed[1], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][30], tailWagRotationMaxes[2], tailWagSpeed[2], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][31], tailWagRotationMaxes[3], tailWagSpeed[3], 2, 0));
        }

        public static void StartTailWagSitting(int mabelToTransform)
        {
            float[][] tailWagRotationMaxes = new float[4][];
            tailWagRotationMaxes[0] = new float[2] { 145, 215 };
            tailWagRotationMaxes[1] = new float[2] { -35, 35 };
            tailWagRotationMaxes[2] = new float[2] { -35, 35 };
            tailWagRotationMaxes[3] = new float[2] { -35, 35 };
            float[] tailStartingRotation = new float[4] { 180, 330, 350, 350 };
            float[] tailWagSpeed = new float[4] { 2f, 2f, -2f, -2f };
            mabelBones[mabelToTransform][28].localRotation = Quaternion.Euler(304.8956f, 167f, tailStartingRotation[0]);
            mabelBones[mabelToTransform][29].localRotation = Quaternion.Euler(6.445f, 0f, tailStartingRotation[1]);
            mabelBones[mabelToTransform][30].localRotation = Quaternion.Euler(7.5996f, 0f, tailStartingRotation[2]);
            mabelBones[mabelToTransform][31].localRotation = Quaternion.Euler(4.5986f, 0f, tailStartingRotation[3]);
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][28], tailWagRotationMaxes[0], tailWagSpeed[0], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][29], tailWagRotationMaxes[1], tailWagSpeed[1], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][30], tailWagRotationMaxes[2], tailWagSpeed[2], 2, 0));
            MelonCoroutines.Start(JointWag(mabelBones[mabelToTransform][31], tailWagRotationMaxes[3], tailWagSpeed[3], 2, 0));
        }

        /*private void ReskinCat()
        {
            if (File.Exists(MelonEnvironment.UserDataDirectory + @"\MabelTheCat\Cat.png"))
            {
                Texture2D texturesLocal = new Texture2D(2, 2);
                byte[] Bytes = File.ReadAllBytes(MelonEnvironment.UserDataDirectory + @"\MabelTheCat\Cat.png");
                ImageConversion.LoadImage(texturesLocal, Bytes);
                texturesLocal.hideFlags = HideFlags.HideAndDontSave;

                SkinnedMeshRenderer meshRenderer = mabel.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                meshRenderer.GetPropertyBlock(block);
                block.SetTexture("_BASEMAP", texturesLocal);
                meshRenderer.SetPropertyBlock(block);

                MelonLogger.Msg("Cat Reskinned");
            }
            else
            {
                MelonLogger.Error("CAT PHOTO NOT FOUND: " + MelonEnvironment.UserDataDirectory + @"\MabelTheCat\Cat.png");
            }
        }
        
        public void SavePose()
        {
            List<string> saveText = new List<string>();
            for (int i = 0; i < mabelBones.Count; i++)
            {
                saveText.Add($"mabelBones[{i}].localRotation = Quaternion.Euler({mabelBones[i].localRotation.eulerAngles.x.ToString(CultureInfo.InvariantCulture)}f, {mabelBones[i].localRotation.eulerAngles.y.ToString(CultureInfo.InvariantCulture)}f, {mabelBones[i].localRotation.eulerAngles.z.ToString(CultureInfo.InvariantCulture)}f);");
            }
            saveText.Add($"spawnedMabel.transform.position = new Vector3(" + spawnedMabel.transform.position.x.ToString(CultureInfo.InvariantCulture) + "f, " + spawnedMabel.transform.position.y.ToString(CultureInfo.InvariantCulture) + "f, " + spawnedMabel.transform.position.z.ToString(CultureInfo.InvariantCulture) + "f);");
            saveText.Add($"spawnedMabel.transform.localRotation = Quaternion.Euler(" + spawnedMabel.transform.localRotation.eulerAngles.x.ToString(CultureInfo.InvariantCulture) + "f, " + spawnedMabel.transform.localRotation.eulerAngles.y.ToString(CultureInfo.InvariantCulture) + "f, " + spawnedMabel.transform.localRotation.eulerAngles.z.ToString(CultureInfo.InvariantCulture) + "f);");
            File.WriteAllLines(@"UserData\MabelTheCat\Pose.txt", saveText);
            MelonLogger.Msg("Pose Saved");
        }

        public void CreateSpheres()
        {
            holdPoints.Clear();
            Shader URP = Shader.Find("Universal Render Pipeline/Lit");
            for (int i = 0; i < mabelBones.Count; i++)
            {
                GameObject holdPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                holdPoint.layer = 22;
                holdPoint.GetComponent<SphereCollider>().isTrigger = true;
                holdPoint.AddComponent<HandColliderChecker>();
                holdPoint.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                holdPoint.GetComponent<Renderer>().material.shader = URP;
                holdPoint.GetComponent<Renderer>().material.color = Color.red;
                holdPoint.transform.parent = mabelBones[i];
                holdPoint.transform.localPosition = Vector3.zero;
                holdPoints.Add(holdPoint);
            }
        }

        [RegisterTypeInIl2Cpp]
        public class HandColliderChecker : MonoBehaviour
        {

            void OnTriggerEnter(Collider other)
            {
                if ((other.gameObject.name != "Bone_HandAlpha_L") && (other.gameObject.name != "Bone_HandAlpha_R"))
                {
                    return;
                }
                this.gameObject.GetComponent<Renderer>().material.color = Color.green;
                if ((other.gameObject.name == "Bone_HandAlpha_L") && (Calls.ControllerMap.LeftController.GetTrigger() > 0.5f) && !leftHandGrabbed)
                {
                    leftHand = other.gameObject;
                    leftHandHeldJoint = this.gameObject.transform.parent.gameObject;
                    leftHandGrabbed = true;
                }
                if ((other.gameObject.name == "Bone_HandAlpha_R") && (Calls.ControllerMap.RightController.GetTrigger() > 0.5f) && !rightHandGrabbed)
                {
                    rightHand = other.gameObject;
                    rightHandHeldJoint = this.gameObject.transform.parent.gameObject;
                    rightHandGrabbed = true;
                }
            }

            void OnTriggerStay(Collider other)
            {
                if (((other.gameObject.name != "Bone_HandAlpha_L") && (other.gameObject.name != "Bone_HandAlpha_R")))
                {
                    return;
                }
                if ((other.gameObject.name == "Bone_HandAlpha_L") && (Calls.ControllerMap.LeftController.GetTrigger() > 0.5f) && !leftHandGrabbed)
                {
                    leftHand = other.gameObject;
                    leftHandHeldJoint = this.gameObject.transform.parent.gameObject;
                    leftHandGrabbed = true;
                }
                if ((other.gameObject.name == "Bone_HandAlpha_R") && (Calls.ControllerMap.RightController.GetTrigger() > 0.5f) && !rightHandGrabbed)
                {
                    rightHand = other.gameObject;
                    rightHandHeldJoint = this.gameObject.transform.parent.gameObject;
                    rightHandGrabbed = true;
                }
            }

            void OnTriggerExit(Collider other)
            {
                if (!((other.gameObject.name != "Bone_HandAlpha_L") && (other.gameObject.name != "Bone_HandAlpha_R")))
                {
                    this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }

        public override void OnFixedUpdate()
        {
            if (leftHandGrabbed)
            {
                if (Calls.ControllerMap.LeftController.GetTrigger() <= 0.5f)
                {
                    leftHandGrabbed = false;
                }
                else
                {
                    leftHandHeldJoint.transform.rotation = leftHand.transform.rotation;
                }
            }
            if (rightHandGrabbed)
            {
                if (Calls.ControllerMap.RightController.GetTrigger() <= 0.5f)
                {
                    rightHandGrabbed = false;
                }
                else
                {
                    rightHandHeldJoint.transform.rotation = rightHand.transform.rotation;
                }
            }
        }

        public override void OnUpdate()
        {
            if ((Input.GetKeyDown(KeyCode.Q)) && (qKeyReleased))
            {
                SavePose();
                qKeyReleased = false;
            }
            if ((Input.GetKeyUp(KeyCode.Q)) && (!qKeyReleased))
            {
                qKeyReleased = true;
            }
        }*/
    }
}
