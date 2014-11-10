GameViewGui.add(new GuiControl(HUD) {
   profile = InvisibleProfile;
   horizSizing = Right;
   vertSizing = Bottom;
   position = "0 0";
   extent = "800 600";
   visible = false;

   new GuiControl([Interaction]) {
      profile = BackgroundProfile;
      position = "-350 50";
      extent = "350 50";

      new GuiBitmapCtrl([InteractionIcon]) {
         visible = false;
      };

      new GuiTextCtrl([InteractionString]) {
         profile = SmallTitleProfile;
         position = "50 10";
         extent = "300 30";
      };
   };
});

HUD.Tweens = Twillex::create();
HUD.Tweens.startUpdates();

function clientCmdInteraction(%text) {
   HUD-->InteractionString.setText(%text);
   %textW = getWord(HUD-->InteractionString.extent, 0);
   HUD-->Interaction.extent = 100 + %textW SPC 50;
   HUD.Tweens.toOnce(200, HUD-->Interaction, "position: 0 50", "ease:sine_out");
}

function clientCmdNoInteraction() {
   %pos = -1 * getWord(Hud-->Interaction.extent, 0) SPC 50;
   HUD.Tweens.toOnce(200, HUD-->Interaction, "position:" SPC %pos, "ease:sine_in");
}
