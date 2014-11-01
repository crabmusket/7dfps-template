new GuiControl(LogoGui) {
   profile = BackgroundProfile;
   horizSizing = "width";
   vertSizing = "height";
   position = "0 0";
   extent = "1024 768";
   minExtent = "8 8";
   wrap = false;

   new GuiTextCtrl() {
      profile = TitleProfile;
      text = "FEED ME";
   };
};
