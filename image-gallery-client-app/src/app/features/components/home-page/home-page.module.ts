// NG
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// APP
import { HomePageComponent } from './home-page.component';
import { SliderModule } from '@shared/components/slider';

@NgModule({
	declarations: [HomePageComponent],
	exports: [HomePageComponent],
	imports: [
        CommonModule,
        FormsModule,
		HttpClientModule,
		RouterModule ,
		SliderModule
	],
})
export class HomePageModule {
}
