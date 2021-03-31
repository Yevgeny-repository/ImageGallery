import { Directive, ElementRef, Renderer, Renderer2 } from '@angular/core';

@Directive({
  selector: '.galleryItemclass'
})
export class GalleryItemClassDirective {

  constructor(private el: ElementRef,
    private renderer: Renderer2) {
//noinspection TypeScriptUnresolvedVariable,TypeScriptUnresolvedFunction
renderer.setStyle(el.nativeElement, 'backgroundColor', 'gray');
    }
}
