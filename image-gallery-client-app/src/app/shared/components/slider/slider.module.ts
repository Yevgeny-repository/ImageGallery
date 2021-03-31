// NG
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

// APP
import { SliderComponent } from './slider.component';
import { GalleryItemClassDirective } from './gallery-item-class.directive';

@NgModule({
	declarations: [SliderComponent, GalleryItemClassDirective],
	exports: [SliderComponent],
	imports: [
		CommonModule,
		RouterModule
	],
})
export class SliderModule {
}
