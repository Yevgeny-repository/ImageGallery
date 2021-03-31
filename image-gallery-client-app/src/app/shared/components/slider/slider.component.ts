// NG
import { Component, 
        ElementRef, 
        EventEmitter, 
        Input, 
        OnChanges, 
        Output, 
        QueryList, 
        SimpleChanges, 
        ViewChild, 
        ViewChildren} from '@angular/core';

// APP
import { GalleryItemClassDirective } from './gallery-item-class.directive';
import { PhotosModel } from '@shared/models';

/**
 * Simple slider component
 */
@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.scss']
})
export class SliderComponent implements OnChanges{
  @Input() photosModel: PhotosModel[] = [];
  @ViewChildren(GalleryItemClassDirective, { read: ElementRef }) slides
    : QueryList<ElementRef<HTMLDivElement>>;
    @ViewChild('gallery',{ static: false }) galleryContainer: ElementRef<HTMLDivElement>;

  /**
	 * Event emitter to emit when slide is selected
	 */
	@Output()
	public onSlideSelected = new EventEmitter<PhotosModel>();

  private slidesIndex = 0;

  get currentSlide(): ElementRef<HTMLDivElement> {
    return this.slides.find((item, index) => index === this.slidesIndex);
  }

  /**
   * Event fired when user click on left arrow
   */
  onClickLeft() {
    this.galleryContainer.nativeElement.scrollLeft -= this.currentSlide.nativeElement.offsetWidth;
    
    if (this.slidesIndex > 0) {
      this.slidesIndex--;
    } 
  }

  /**
   * Event fired when user click on right arrow
   */
  onClickRight() {
    this.galleryContainer.nativeElement.scrollLeft += this.currentSlide.nativeElement.offsetWidth;

    if (this.slidesIndex < this.slides.length - 1) {
      this.slidesIndex++
    }
  }

  /**
   * handleSlideSelected event emitter
   * @param model 
   */
  handleSlideSelected(model: PhotosModel) {
    this.onSlideSelected.emit(model);
  }

  /**
   * OnChanges hook
   * here we check if photos model was changed then scrollLeft will return to 0
   * @param changes 
   */
  ngOnChanges(changes: SimpleChanges): void {
    this.slidesIndex = 0;
    if (changes['photosModel'].previousValue)
      this.galleryContainer.nativeElement.scrollLeft = 0;
   }
}