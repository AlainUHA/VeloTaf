using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    [Serializable]
    public class JoursRoules
    {
        public List<string> listeDates;
    }


    public class JoursVelo : MonoBehaviour
    {
        private int _nbJours;
        public TextMeshProUGUI textNbJours;
        public TextMeshProUGUI textDate;
        private DateTime _today;
        private string _date;
        public JoursRoules joursRoules;
        private string _saveFile;
        public GameObject datesPanel;
        private string _chaine;//la chaine qui contient les dates
        public GameObject datePrefab;
        private GameObject _dateClone;
        private TMP_InputField _inputField;
        private bool _reinitialiser;
        public GameObject panelMenu;

        // Start is called before the first frame update
        private void Start()
        {
            _nbJours = PlayerPrefs.GetInt("nbJours",0);
            textNbJours.text = _nbJours.ToString();
            _date = PlayerPrefs.GetString("date", "/");
            textDate.text = _date;
            joursRoules.listeDates = new List<String>();
            _saveFile = Application.persistentDataPath + "/joursRoules.json";
            if (File.Exists(_saveFile))
            {
                // Read the entire file and save its contents.
                string fileContents = File.ReadAllText(_saveFile);

                // Deserialize the JSON data 
                //  into a pattern matching the Actions class.
                joursRoules = JsonUtility.FromJson<JoursRoules>(fileContents);
            }
            _reinitialiser = false;
        }

        public void AjouteJour()
        {
            _nbJours++;
            SauveJour();
            SauveDate();
        }

        public void RetireJour()
        {
            _nbJours--;
            SauveJour();
            SupprimeDerniereDate();
        }

        private void SupprimeDerniereDate()
        {
            joursRoules.listeDates.RemoveAt(joursRoules.listeDates.Count -1);
            _date = joursRoules.listeDates[^1];
            textDate.text = _date;
            PlayerPrefs.SetString("date", _date);
            Debug.Log("Nombre d'éléments dans la liste : " + joursRoules.listeDates.Count);
            var joursRoulesJson = JsonUtility.ToJson(joursRoules);
            //Debug.Log("Jours roulés :" + joursRoulesJson);
            File.WriteAllText(_saveFile, joursRoulesJson);
        }

        public void SupprimerDate(string date)
        {
            joursRoules.listeDates.Remove(date);
        }

        private void SauveJour()
        {
            textNbJours.text = _nbJours.ToString();
            PlayerPrefs.SetInt("nbJours", _nbJours);
        }

        private void SauveDate()
        {
            if (!_reinitialiser)
            {
                //on ajoute la date du jour
                _today = DateTime.Today;
                _date = _today.ToString("dd-MM-yyyy");
                joursRoules.listeDates.Add(_date);
            }
            else
            {
                _reinitialiser = false;
                _date = "/";
                //on vient de réinitialiser, on repasse la variable à false
            }
            textDate.text = _date;
            PlayerPrefs.SetString("date", _date);
            var joursRoulesJson = JsonUtility.ToJson(joursRoules);
            File.WriteAllText(_saveFile, joursRoulesJson);
        }

        public void ReinitialiserEtArchiver()
        {
            _nbJours = 0;
            SauveJour();
            _reinitialiser = true;
            joursRoules.listeDates.Clear();
            SauveDate();
        }

        public void QuitterApp()
        {
            Application.Quit();
        }

        public void ListerDate()
        {
            ModifierTailleDates();
            List<string> dates = joursRoules.listeDates;
            foreach (string d in dates)
            {
                _dateClone = Instantiate(datePrefab, datesPanel.transform);
                _dateClone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d;
            }
        }

        public void QuitterDates()
        {
            int i = 0;
            foreach (Transform date in datesPanel.transform)
            {
                joursRoules.listeDates[i]= date.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
                Destroy(date.gameObject);
                i++;
            }
            var joursRoulesJson = JsonUtility.ToJson(joursRoules);
            File.WriteAllText(_saveFile, joursRoulesJson);

            datesPanel.transform.parent.parent.parent.parent.gameObject.SetActive(false);
        }

        private void ModifierTailleDates()
        {
            var nbdates = joursRoules.listeDates.Count;
            // ReSharper disable once PossibleLossOfFraction
            var h = (int)(Mathf.Round(nbdates / 3)+1) * 70;
            var w = datesPanel.GetComponent<RectTransform>().sizeDelta.x;
            datesPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(w,h);
            datesPanel.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, h);
        }
        public void AfficherMenu()
        {
            panelMenu.SetActive(!panelMenu.activeSelf);
        }
    }
}