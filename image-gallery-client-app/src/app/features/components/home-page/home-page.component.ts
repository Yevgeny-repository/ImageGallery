// NG
import { Component, OnInit } from '@angular/core';

// VENDOR
import { SubSink } from 'subsink';
import { flatMap } from 'rxjs/operators';
import { interval, Subscription } from 'rxjs';

// APP
import { PhotosModel } from '@shared/models';
import { PhotosService } from '@shared/services/users/photos-shared.service';

/**
 * Home page component
 */
@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  
  photosModel: PhotosModel[] = [];
  selectedPhoto: PhotosModel;
  private interval$: Subscription;
  private subs = new SubSink();

  /**
   * Constuctor 
   * Here PhotosService injected
   * @param photosService 
   */
  constructor(private photosService: PhotosService) { }

  /**
   * OnInit Hook
   * Here we call get pthotos api every 30 seconds
   */
  ngOnInit() {
    if (this.interval$)
      this.interval$.unsubscribe();

    this.subs.sink = this.photosService.getPhotos()
    .subscribe(data => this.photosSub(data));

    this.interval$ =  interval(30 * 1000)
    .pipe(
        flatMap(() => this.photosService.getPhotos())
    )
    .subscribe(data => this.photosSub(data));
  }

  /**
   * Subscribe to getPhotos
   * @param data 
   */
  photosSub(data: PhotosModel[]): void {
    this.photosModel = data;
    this.selectedPhoto = this.photosModel[2];
  }
  
  /**
   * OnOestroy Hook
   * Here we unsubscribe from getPhotos and interval
   */
  public ngOnOestroy() {
    this.subs.unsubscribe();
    this.interval$.unsubscribe();
  }

  /**
   * This function will fired when user click on one of the images in slider
   * @param data 
   */
  selectPhoto(data: PhotosModel): void {
    this.selectedPhoto = data;
  }
}
