singleton Material(TrainingLight)
{
   mapTo = "TrainingLight";
   diffuseColor[0] = "0.827451 0.776471 0.623529 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
   glow = true;
   emissive = true;
};

singleton Material(TrainingWalls)
{
   mapTo = "TrainingWalls";
   diffuseColor[0] = "0.64 0.64 0.64 1";
   specular[0] = "0.5 0.5 0.5 1";
   specularPower[0] = "38";
   doubleSided = "0";
   translucentBlendOp = "None";
   specularStrength[0] = "0.75";
};
