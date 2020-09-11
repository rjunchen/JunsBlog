import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileEditorComponent } from './profile-editor.component';
import { ProfileModule } from '../profile.module';
import { AppModule } from 'src/app/app.module';
import { User } from 'src/app/models/authentication/user';

describe('ProfileEditorComponent', () => {
  let component: ProfileEditorComponent;
  let fixture: ComponentFixture<ProfileEditorComponent>;
 
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfileEditorComponent ],
      imports:[ProfileModule, AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfileEditorComponent);
    component = fixture.componentInstance;
    let user = new User();
    user.createdOn = new Date().toDateString();
    user.email = 'Test@test.com',
    user.id = '1234567890',
    user.image = './assets/avatar.png',
    user.name = 'tester',
    user.role = 'user',
    user.type = 'local',
    user.updatedOn = new Date().toDateString();
    component.user = user;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
