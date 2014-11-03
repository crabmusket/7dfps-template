new SimGroup(LevelGroup) {
   new LevelInfo(TheLevelInfo) {
      canvasClearColor = "CornflowerBlue";
   };
   new GroundPlane(TheGround) {
      position = "0 0 0";
      material = BlankWhite;
   };
   new Sun(TheSun) {
      azimuth = 230;
      elevation = 45;
      color = "White";
      ambient = "0.1 0.1 0.1";
      castShadows = true;
   };
};
