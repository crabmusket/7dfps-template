new GuiControlProfile(InvisibleProfile : GuiDefaultProfile) {
   fillColor = "0 0 0 0";
   opaque = false;
   modal = false;
};

new GuiControlProfile(TextProfile : GuiDefaultProfile) {
   fontSize = 18;
   fontColor = "White";
   fontColorHL = "White";
   fillColor = "Black";
   fillColorHL = "Black";
};

new GuiControlProfile(TitleProfile : TextProfile) {
   fontType = "basic title font";
   fontSize = 36;
};

new GuiControlProfile(LevelListProfile : TitleProfile) {
   fontSize = 26;
};

new GuiControlProfile(LevelListInvertedProfile : LevelListProfile) {
   fontColor = "Black";
   fontColorHL = "Black";
   fillColor = "White";
   fillColorHL = "White";
};

new GuiControlProfile(DialogProfile : GuiDefaultProfile) {
   fillColor = "0 0 0 0";
   opaque = false;
};

new GuiControlProfile(BackgroundProfile : GuiDefaultProfile) {
   fillColor = "Black";
   opaque = true;
};
