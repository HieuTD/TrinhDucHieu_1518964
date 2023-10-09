/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CreateOrEditProductComponent } from './create-or-edit-product.component';

describe('CreateOrEditProductComponent', () => {
  let component: CreateOrEditProductComponent;
  let fixture: ComponentFixture<CreateOrEditProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateOrEditProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrEditProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
