new GuiControl(MainMenuGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "1024 768";
   minExtent = "8 8";
   wrap = false;

   new GuiButtonCtrl() {
      profile = GuiDefaultProfile;
      position = "0 0";
      buttonType = PushButton;
      //command = "play();";
      text = "PLAY";
   };

   new GuiButtonCtrl() {
      profile = GuiDefaultProfile;
      position = "0 60";
      buttonType = PushButton;
      //command = "connect();";
      text = "JOIN";
   };
};
