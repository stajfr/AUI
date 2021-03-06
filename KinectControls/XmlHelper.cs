﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace KinectControls
{
    public class XmlHelper
    {
        public class Story
        {
            public int storyID { get; set; }
            public string vidUrl { get; set; }
            public List<Time> time = new List<Time>();
            public double duration { get; set; }
            public List<ArduinoActions> arduinoActions = new List<ArduinoActions>();
            public List<Choice> choice = new List<Choice>();
        }
        public class ArduinoActions
        {
            public List<Fan> listFan = new List<Fan>();
            public List<Led> listLed = new List<Led>();
        }
        public class Time
        {
            public double min { get; set; }
            public double sec { get; set; }
        }
        public class Fan
        {
            public List<Time> time = new List<Time>();
            public Boolean onStatus { get; set; }
        }
        public class Led
        {
            public List<Time> time = new List<Time>();
            public int red { get; set; }
            public int green { get; set; }
            public int blue { get; set; }
        }
        public class Choice
        {
            public List<KinectButton> listKinectButton = new List<KinectButton>();
            public List<Speech> listSpeech = new List<Speech>();
            public String type { get; set; }
        }
        
        public class Speech
        {
            public List<Time> time = new List<Time>();
            public String text { get; set; }
        }

        public class KinectButton
        {
            public int btnID { get; set; }
            public List<Time> time = new List<Time>();
            public int duration { get; set; }
            public String imageURL { get; set; }
            public List<ArduinoActions> arduinoActions = new List<ArduinoActions>();
            public List<Speech> listSpeech = new List<Speech>();
            public Boolean rightChoice { get; set; }
            public List<Color> listColor = new List<Color>();
        }

        public class Color
        {
            public double red { get; set; }
            public double green { get; set; }
            public double blue { get; set; } 
        }

        public static List<Story> GetStoryData()
        {
            XElement xmlDoc = XElement.Load("../../../Stories.xml");//"C:/Users/saeed/Documents/GitHub/AUI/KinectControls/Stories.xml");
            List<Story> Stories = getStories(xmlDoc);
            return Stories;
        }


        private static List<Story> getStories(XElement root)
        {
            return new List<Story>(from story in root.Descendants("story")
                   select new Story
                   {
                       storyID = Convert.ToInt32(story.Element("ID").Value),
                       vidUrl = story.Element("VideoUrl").Value,
                       time = getTimes(story),
                       duration = Convert.ToInt32(story.Element("Duration").Value),
                       arduinoActions = getArduinoActions(story),
                       choice = getChoices(story)
                   });
        }

        private static List<ArduinoActions> getArduinoActions(XElement root)
        {
            return new List<ArduinoActions>(from arduinoAction in root.Descendants("ArduinoActions")
                                            select new ArduinoActions
                                            {
                                                listFan = getFans(arduinoAction),
                                                listLed = getLeds(arduinoAction),
                                            });
        }

        private static List<Fan> getFans(XElement root)
        {
            return new List<Fan>(from listFan in root.Descendants("Fan")
                                 select new Fan
                                 {
                                     time = getTimes(listFan),
                                     onStatus = Convert.ToBoolean(listFan.Element("ON").Value)
                                 });
        }

        private static List<Led> getLeds(XElement root)
        {
            return new List<Led>(from listLed in root.Descendants("Led")
                                 select new Led
                                 {
                                     time = getTimes(listLed),
                                     red = Convert.ToInt32(listLed.Element("StatusRed").Value),
                                     green = Convert.ToInt32(listLed.Element("StatusGreen").Value),
                                     blue = Convert.ToInt32(listLed.Element("StatusBlue").Value)
                                 });
        }

        private static List<Choice> getChoices(XElement root)
        {
            return new List<Choice>(from choice in root.Descendants("Choice")
                                    select new Choice
                                    {
                                        listKinectButton = getKinectButtons(choice),
                                        listSpeech = getSpeeches(choice),
                                        type = choice.Element("Type").Value
                                    });
        }

        private static List<Speech> getSpeeches(XElement root)
        {
            return new List<Speech>(from listSpeech in root.Descendants("Speech")
                                    select new Speech
                                    {
                                        time = getTimes(listSpeech),
                                        text = listSpeech.Element("Text").Value
                                    });
        }

        private static List<KinectButton> getKinectButtons(XElement root)
        {
            return new List<KinectButton>(from kinectButton in root.Descendants("KinectButton")
                                          select new KinectButton
                                          {
                                              btnID = Convert.ToInt32(kinectButton.Element("KinecButtonID").Value),
                                              time = getTimes(kinectButton),
                                              duration = Convert.ToInt32(kinectButton.Element("Duration").Value),
                                              imageURL = kinectButton.Element("ImageURL").Value,
                                              listSpeech = getSpeeches(kinectButton),
                                              arduinoActions = getArduinoActions(kinectButton),
                                              rightChoice = Convert.ToBoolean(kinectButton.Element("RightChoice").Value),
                                              listColor = getColors(kinectButton)
                                          });
        }
        
        private static List<Color> getColors(XElement root)
        {
            return new List<Color>(from color in root.Descendants("Color")
                                   select new Color
                                   {
                                       red = Convert.ToDouble(color.Element("Red").Value),
                                       green = Convert.ToDouble(color.Element("Green").Value),
                                       blue = Convert.ToDouble(color.Element("Blue").Value)
                                   });
        }

        private static List<Time> getTimes(XElement root)
        {
            return new List<Time>(from time in root.Descendants("Time")
                                  select new Time
                                  {
                                      min = Convert.ToDouble(time.Element("Min").Value),
                                      sec = Convert.ToDouble(time.Element("Sec").Value)
                                  });
        }
    }
}
