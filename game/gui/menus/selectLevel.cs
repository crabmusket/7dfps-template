new GuiControl(ChooseLevelGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "1024 768";
   minExtent = "8 8";
   wrap = false;

   new GuiTextCtrl() {
      profile = TitleProfile;
      text = "CHOOSE LEVEL";
      position = "20 20";
      extent = "250 30";
   };

   new GuiButtonCtrl() {
      profile = TitleProfile;
      text = "PLAY";
      position = "20 70";
      command = "GuiEvents.postEvent(EvtStartGame);";
   };
};

GuiEvents.registerEvent(EvtStartGame);
