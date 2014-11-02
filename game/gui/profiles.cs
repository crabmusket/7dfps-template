new GuiControlProfile(TitleProfile : GuiDefaultProfile) {
   fontType = "basic title font";
   fontSize = 36;
   fontColor = "White";
   fontColorHL = "White";
   fillColor = "Black";
   fillColorHL = "Black";
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

new GuiControlProfile(BackgroundProfile : GuiDefaultProfile) {
   fillColor = "Black";
   opaque = true;
};
