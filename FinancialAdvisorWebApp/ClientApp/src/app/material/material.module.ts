import { NgModule } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';


const MaterialComponenets = [
  MatSidenavModule
];

@NgModule({
  imports: [MaterialComponenets],
  exports: [MaterialComponenets]
})
export class MaterialModule { }
