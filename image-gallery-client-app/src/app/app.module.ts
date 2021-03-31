// NG
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

// APP
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from '@shared/components/layout';
import { SliderModule } from '@shared/components/slider';
import { FeatureModule } from './features';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FeatureModule,
    LayoutModule,
    SliderModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
